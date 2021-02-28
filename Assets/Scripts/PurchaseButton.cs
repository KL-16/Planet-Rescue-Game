using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    public enum PurchaseType {removeAds, gamePack1, pregamePack1, allBoostersPack1, gamePack3, pregamePack3, allBoostersPack3, gamePack5, pregamePack5, allBoostersPack5, 
        gamePack15, pregamePack15, allBoostersPack15 };
    public PurchaseType purchaseType;

    public Text priceText;

    private void Start()
    {
        StartCoroutine(LoadPriceRoutine());
    }

    public void ClickPurchaseButton()
    {
        switch (purchaseType)
        {
            case PurchaseType.removeAds:
                IAPManager.instance.BuyRemoveAdds();
                break;
            case PurchaseType.gamePack1:
                IAPManager.instance.BuyGamePack1();
                break;
            case PurchaseType.pregamePack1:
                IAPManager.instance.BuyPregamePack1();
                break;
            case PurchaseType.allBoostersPack1:
                IAPManager.instance.BuyAllBoostersPack1();
                break;
            case PurchaseType.gamePack3:
                IAPManager.instance.BuyGamePack3();
                break;
            case PurchaseType.pregamePack3:
                IAPManager.instance.BuyPregamePack3();
                break;
            case PurchaseType.allBoostersPack3:
                IAPManager.instance.BuyAllBoostersPack3();
                break;
            case PurchaseType.gamePack5:
                IAPManager.instance.BuyGamePack5();
                break;
            case PurchaseType.pregamePack5:
                IAPManager.instance.BuyPregamePack5();
                break;
            case PurchaseType.allBoostersPack5:
                IAPManager.instance.BuyAllBoostersPack5();
                break;
            case PurchaseType.gamePack15:
                IAPManager.instance.BuyGamePack15();
                break;
            case PurchaseType.pregamePack15:
                IAPManager.instance.BuyPregamePack15();
                break;
            case PurchaseType.allBoostersPack15:
                IAPManager.instance.BuyAllBoostersPack15();
                break;
        }
    }

    private IEnumerator LoadPriceRoutine()
    {
        while (!IAPManager.instance.IsInitialized())
        {
            yield return null;
        }

        string loadedPrice = "";

        switch (purchaseType)
        {
            case PurchaseType.removeAds:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.removeAds);
                break;
            case PurchaseType.gamePack1:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.gamePack1);
                break;
            case PurchaseType.pregamePack1:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.pregamePack1);
                break;
            case PurchaseType.allBoostersPack1:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.allBoostersPack1);
                break;
            case PurchaseType.gamePack3:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.gamePack3);
                break;
            case PurchaseType.pregamePack3:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.pregamePack3);
                break;
            case PurchaseType.allBoostersPack3:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.allBoostersPack3);
                break;
            case PurchaseType.gamePack5:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.gamePack5);
                break;
            case PurchaseType.pregamePack5:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.pregamePack5);
                break;
            case PurchaseType.allBoostersPack5:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.allBoostersPack5);
                break;
            case PurchaseType.gamePack15:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.gamePack15);
                break;
            case PurchaseType.pregamePack15:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.pregamePack15);
                break;
            case PurchaseType.allBoostersPack15:
                loadedPrice = IAPManager.instance.GetProductPrizeFromStore(IAPManager.instance.allBoostersPack15);
                break;
        }

        priceText.text = loadedPrice;
    }
}
