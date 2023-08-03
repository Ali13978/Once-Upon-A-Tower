using TMPro;
using UnityEngine;

public class CharacterSelectItem : MonoBehaviour
{
	public Renderer CharacterFrame;

	public GuiButton SelectButton;

	public GameObject PlateContent;

	public TextMeshPro NamePlate;

	public Transform StatesParent;

	public GameObject Effect;

	public GameObject Flash;

	public GameObject Frame;

	public GameObject MaskComplement;

	public GameObject Mask;

	public Animator Animator;

	private GameObject[] adjustables;

	private Vector3[] adjustableLocalPositions;

	public string CharacterName => base.name.Replace("Select", string.Empty);

	private void Start()
	{
		Init();
	}

	private void Init()
	{
		if (adjustables == null || adjustables.Length <= 0)
		{
			Character character = Characters.ByName(CharacterName);
			if (character != null)
			{
				NamePlate.text = character.DisplayName;
			}
			adjustables = new GameObject[4]
			{
				PlateContent,
				Flash,
				Frame,
				Mask
			};
			adjustableLocalPositions = new Vector3[adjustables.Length];
			for (int i = 0; i < adjustables.Length; i++)
			{
				adjustableLocalPositions[i] = adjustables[i].transform.localPosition;
			}
			SelectButton.HighlightRenderer = Frame.GetComponent<MeshRenderer>();
		}
	}

	public void ChangePose()
	{
		if (Animator != null)
		{
			Animator.SetTrigger("change_pose");
		}
	}

	public void SetupFrame(string name, Material material)
	{
		Init();
		if (CharacterFrame == null)
		{
			UnityEngine.Debug.LogError("Null frame in CharacterSelectItem " + name);
			return;
		}
		Material[] sharedMaterials = CharacterFrame.sharedMaterials;
		for (int i = 0; i < sharedMaterials.Length; i++)
		{
			sharedMaterials[i] = material;
		}
		CharacterFrame.sharedMaterials = sharedMaterials;
		for (int j = 0; j < StatesParent.childCount; j++)
		{
			Transform child = StatesParent.GetChild(j);
			if (child.name != "Effect")
			{
				child.gameObject.SetActive(value: false);
			}
		}
		StatesParent.Find(name).gameObject.SetActive(value: true);
	}

	public void SetupFrame()
	{
		Init();
		if (SaveGame.Instance.CharacterOwned(CharacterName))
		{
			if (SaveGame.Instance.CharacterRescued(CharacterName))
			{
				SetupFrame("Free", Gui.Views.CharacterSelect.FreeMaterial);
			}
			else
			{
				SetupFrame("Unlocked", Gui.Views.CharacterSelect.UnlockedMaterial);
			}
		}
		else
		{
			SetupFrame("Locked", Gui.Views.CharacterSelect.LockedMaterial);
		}
	}

	public void SetupBuy()
	{
		Init();
		MaskComplement.SetActive(value: true);
	}

	public void SetupGet()
	{
		Init();
		Vector3 vector = new Vector3(0f, 0f, 3f);
		for (int i = 0; i < adjustables.Length; i++)
		{
			adjustables[i].transform.position -= vector;
			if (adjustables[i].name == "PlateContent")
			{
				adjustables[i].transform.position -= 2f * vector;
			}
		}
		Effect.SetActive(value: true);
		Flash.SetActive(value: true);
		Mask.SetActive(value: true);
		MaskComplement.SetActive(value: true);
	}

	public void Reset()
	{
		if (adjustables != null)
		{
			for (int i = 0; i < adjustables.Length; i++)
			{
				adjustables[i].transform.localPosition = adjustableLocalPositions[i];
			}
		}
		Effect.SetActive(value: false);
		Flash.SetActive(value: false);
		Mask.SetActive(value: false);
		MaskComplement.SetActive(value: false);
	}
}
