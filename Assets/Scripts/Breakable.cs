using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WorldObject))]
public class Breakable : MonoBehaviour
{
	public ParticleSystem BreakParticles;

	public ParticleSystem DownBreakParticles;

	public ParticleSystem LeftBreakParticles;

	public ParticleSystem RightBreakParticles;

	public ParticleSystem UpBreakParticles;

	public List<GameObject> DisableOnBreak;

	public List<GameObject> ScaleOnBreak;

	public List<ParticleSystem> StopOnBreak;

	public List<Renderer> FlashOnBreak;

	public int FlashFrames = 1;

	public Texture2D FlashRamp;

	public AudioSource BreakAudio;

	private WorldObject wo;

	private IEnumerator checkRagdollDistanceCoroutine;

	private List<GameObject> FireEffect = new List<GameObject>();

	private BoxCollider Collider => wo.Collider;

	private Animator Animator => wo.Animator;

	public bool Broken
	{
		get;
		private set;
	}

	private void Start()
	{
		wo = GetComponent<WorldObject>();
		if (Animator != null && Animator.enabled)
		{
			DisableRagdoll(Animator.gameObject, Animator);
		}
	}

	public void Break(Vector3 direction, WorldObject breaker = null)
	{
		if (!Broken)
		{
			Broken = true;
			wo = GetComponent<WorldObject>();
			if (breaker is Digger)
			{
				SingletonMonoBehaviour<MissionManager>.Instance.NotifyBreak(wo, direction);
			}
			GetComponent<Collider>().enabled = false;
			if (BreakAudio != null && base.gameObject.activeInHierarchy)
			{
				BreakAudio.Play();
			}
			wo.RemoveCoord();
			wo.DisableWithComponents();
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(BreakCoroutine(direction));
			}
			else
			{
				EndBreak(direction);
			}
			if (Animator != null)
			{
				EnableRagdoll(Animator.gameObject, direction, Animator);
			}
		}
	}

	public IEnumerator Flash()
	{
		if (FlashFrames != 0 && !(FlashRamp == null))
		{
			MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
			if (FlashRamp != null)
			{
				propertyBlock.SetTexture("_LightRamp", FlashRamp);
			}
			foreach (Renderer item in FlashOnBreak)
			{
				item.SetPropertyBlock(propertyBlock);
			}
			for (int i = 0; i < FlashFrames; i++)
			{
				yield return null;
			}
			foreach (Renderer item2 in FlashOnBreak)
			{
				item2.SetPropertyBlock(null);
			}
		}
	}

	private IEnumerator BreakCoroutine(Vector3 direction)
	{
		yield return Flash();
		EndBreak(direction);
	}

	private void EndBreak(Vector3 direction)
	{
		ParticleSystem particleSystem = ChooseBreakParticles(direction);
		if ((bool)particleSystem && base.gameObject.activeInHierarchy)
		{
			particleSystem.gameObject.SetActive(value: true);
			particleSystem.Play();
		}
		foreach (GameObject item in DisableOnBreak)
		{
			if (item == null)
			{
				UnityEngine.Debug.LogError("Null object in DisableOnBreak on " + base.name, this);
			}
			else
			{
				item.SetActive(value: false);
			}
		}
		foreach (GameObject item2 in ScaleOnBreak)
		{
			if (item2 == null)
			{
				UnityEngine.Debug.LogError("Null object in ScaleOnBreak on " + base.name, this);
			}
			else
			{
				item2.transform.localScale = Vector3.zero;
			}
		}
		foreach (ParticleSystem item3 in StopOnBreak)
		{
            ParticleSystem.EmissionModule emissionModule = item3.emission;
			emissionModule.enabled = false;
		}
	}

	private ParticleSystem ChooseBreakParticles(Vector3 direction)
	{
		if (direction == Vector3.right && (bool)RightBreakParticles)
		{
			return RightBreakParticles;
		}
		if (direction == Vector3.left && (bool)LeftBreakParticles)
		{
			return LeftBreakParticles;
		}
		if (direction == Vector3.up && (bool)UpBreakParticles)
		{
			return UpBreakParticles;
		}
		if (direction == Vector3.down && (bool)DownBreakParticles)
		{
			return DownBreakParticles;
		}
		return BreakParticles;
	}

	public void Unbreak()
	{
		if (Broken)
		{
			Broken = false;
			foreach (GameObject item in DisableOnBreak)
			{
				if (item == null)
				{
					UnityEngine.Debug.LogError("Null object in DisableOnBreak on " + base.name, this);
				}
				else
				{
					item.SetActive(value: true);
				}
			}
			foreach (GameObject item2 in ScaleOnBreak)
			{
				if (item2 == null)
				{
					UnityEngine.Debug.LogError("Null object in ScaleOnBreak on " + base.name, this);
				}
				else
				{
					item2.transform.localScale = Vector3.one;
				}
			}
			foreach (ParticleSystem item3 in StopOnBreak)
            {
                ParticleSystem.EmissionModule emissionModule = item3.emission;
                emissionModule.enabled = true;
			}
			if ((bool)BreakParticles)
			{
				BreakParticles.gameObject.SetActive(value: false);
				BreakParticles.Clear();
				BreakParticles.Stop();
			}
			for (int i = 0; i < FireEffect.Count; i++)
			{
				FireEffect[i].SetActive(value: false);
			}
			GetComponent<Collider>().enabled = true;
			wo.EnableWithComponents();
			if (Animator != null)
			{
				DisableRagdoll(Animator.gameObject, Animator);
			}
		}
	}

	public void LightFire()
	{
		if (FireEffect.Count > 0)
		{
			for (int i = 0; i < FireEffect.Count; i++)
			{
				FireEffect[i].SetActive(value: true);
				FireEffect[i].GetComponent<ParticleSystem>().Play();
			}
			return;
		}
		for (int j = 0; j < FlashOnBreak.Count; j++)
		{
			Renderer renderer = FlashOnBreak[j];
			GameObject gameObject = UnityEngine.Object.Instantiate(SingletonMonoBehaviour<Game>.Instance.ObjectOnFireParticles.gameObject);
			ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
			component.transform.parent = renderer.transform;
			component.transform.localPosition = Vector3.zero;
			ParticleSystem.ShapeModule shape = component.shape;
			shape.skinnedMeshRenderer = (renderer as SkinnedMeshRenderer);
			shape.meshRenderer = (renderer as MeshRenderer);
			component.Play();
			FireEffect.Add(gameObject);
		}
	}

	protected void DisableRagdoll(GameObject root, Animator animator)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		if (animator != null)
		{
			animator.enabled = true;
		}
		Collider.enabled = true;
		Rigidbody[] componentsInChildren = root.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in componentsInChildren)
		{
			rigidbody.GetComponent<Collider>().enabled = false;
			rigidbody.isKinematic = true;
			rigidbody.detectCollisions = false;
		}
		if (checkRagdollDistanceCoroutine != null)
		{
			StopCoroutine(checkRagdollDistanceCoroutine);
			checkRagdollDistanceCoroutine = null;
		}
	}

	public void EnableRagdoll(GameObject root, Vector3 direction, Animator animator)
	{
		if (animator != null)
		{
			animator.enabled = false;
		}
		if (Collider != null)
		{
			Collider.enabled = false;
		}
		int layer = LayerMask.NameToLayer("Ragdoll");
		if (root == null || root.gameObject == null)
		{
			return;
		}
		Rigidbody[] componentsInChildren = root.gameObject.GetComponentsInChildren<Rigidbody>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Rigidbody rigidbody = componentsInChildren[i];
			Collider component = rigidbody.GetComponent<Collider>();
			if ((bool)component)
			{
				component.enabled = true;
			}
			rigidbody.gameObject.layer = layer;
			rigidbody.isKinematic = false;
			rigidbody.detectCollisions = true;
			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce((direction + Vector3.up + Vector3.back * 4f) * UnityEngine.Random.Range(2f, 3f), ForceMode.Impulse);
			rigidbody.angularVelocity = UnityEngine.Random.onUnitSphere * 0.5f;
			if (rigidbody.name == "Joitn_Weapon_root")
			{
				rigidbody.AddForce((direction + Vector3.up + Vector3.back * 4f) * UnityEngine.Random.Range(2f, 3f), ForceMode.Impulse);
				rigidbody.angularVelocity = UnityEngine.Random.onUnitSphere * 5f;
			}
			if (i == 0 && rigidbody.gameObject.activeInHierarchy)
			{
				checkRagdollDistanceCoroutine = CheckRagdollDistance(rigidbody.gameObject);
				StartCoroutine(checkRagdollDistanceCoroutine);
			}
		}
	}

	private IEnumerator CheckRagdollDistance(GameObject root)
	{
		Vector3 startPosition = root.transform.position;
		while (Vector3.Distance(startPosition, root.transform.position) < 1000f)
		{
			yield return null;
		}
		root.SetActive(value: false);
		checkRagdollDistanceCoroutine = null;
	}
}
