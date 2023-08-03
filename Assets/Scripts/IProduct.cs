using UnityEngine.Purchasing;

public interface IProduct
{
	string ReferenceName
	{
		get;
	}

	string ProductId
	{
		get;
	}

	ProductType ProductType
	{
		get;
	}
}
