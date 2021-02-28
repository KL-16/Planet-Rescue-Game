using System;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    //public BannerAds bannerAds;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Step 1 create your products

    public string removeAds = "remove_ads";
    public string gamePack1 = "game_pack1";
    public string pregamePack1 = "pregame_pack1";
    public string allBoostersPack1 = "allboosters_pack1";

    public string gamePack3 = "game_pack3";
    public string pregamePack3 = "pregame_pack3";
    public string allBoostersPack3 = "allboosters_pack3";

    public string gamePack5 = "game_pack5";
    public string pregamePack5 = "pregame_pack5";
    public string allBoostersPack5 = "allboosters_pack5";

    public string gamePack15 = "game_pack15";
    public string pregamePack15 = "pregame_pack15";
    public string allBoostersPack15 = "allboosters_pack15";

    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct(removeAds, ProductType.NonConsumable);
        builder.AddProduct(gamePack1, ProductType.Consumable);
        builder.AddProduct(pregamePack1, ProductType.Consumable);
        builder.AddProduct(allBoostersPack1, ProductType.Consumable);

        builder.AddProduct(gamePack3, ProductType.Consumable);
        builder.AddProduct(pregamePack3, ProductType.Consumable);
        builder.AddProduct(allBoostersPack3, ProductType.Consumable);

        builder.AddProduct(gamePack5, ProductType.Consumable);
        builder.AddProduct(pregamePack5, ProductType.Consumable);
        builder.AddProduct(allBoostersPack5, ProductType.Consumable);

        builder.AddProduct(gamePack15, ProductType.Consumable);
        builder.AddProduct(pregamePack15, ProductType.Consumable);
        builder.AddProduct(allBoostersPack15, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods
    public void BuyRemoveAdds()
    {
        BuyProductID(removeAds);
    }

    public void BuyGamePack1()
    {
        BuyProductID(gamePack1);
    }

    public void BuyPregamePack1()
    {
        BuyProductID(pregamePack1);
    }

    public void BuyAllBoostersPack1()
    {
        BuyProductID(allBoostersPack1);
    }

    public void BuyGamePack3()
    {
        BuyProductID(gamePack3);
    }

    public void BuyPregamePack3()
    {
        BuyProductID(pregamePack3);
    }

    public void BuyAllBoostersPack3()
    {
        BuyProductID(allBoostersPack3);
    }

    public void BuyGamePack5()
    {
        BuyProductID(gamePack5);
    }

    public void BuyPregamePack5()
    {
        BuyProductID(pregamePack5);
    }

    public void BuyAllBoostersPack5()
    {
        BuyProductID(allBoostersPack5);
    }

    public void BuyGamePack15()
    {
        BuyProductID(gamePack15);
    }

    public void BuyPregamePack15()
    {
        BuyProductID(pregamePack15);
    }

    public void BuyAllBoostersPack15()
    {
        BuyProductID(allBoostersPack15);
    }

    public string GetProductPrizeFromStore(string id)
    {
        if(m_StoreController != null && m_StoreController.products != null)
        {
            return m_StoreController.products.WithID(id).metadata.localizedPriceString;
        }
        else
        {
            return "";
        }
    }

    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, removeAds, StringComparison.Ordinal))
        {
           // Debug.Log("Adds Removed - restart game");
            //bannerAds.HideBannerAdd();
            LoadInfo.adsRemoved = true;
            LoadInfo.SaveNonConsumable(LoadInfo.adsRemoved, "removeAds");          
        }

        else if (String.Equals(args.purchasedProduct.definition.id, gamePack1, StringComparison.Ordinal))
        {
            //Debug.Log("Game Pack 1");
            ++LoadInfo.booster1No;
            ++LoadInfo.booster4No;
            ++LoadInfo.booster5No;
            LoadInfo.Save(LoadInfo.booster1No, "onePieceRemove");
            LoadInfo.Save(LoadInfo.booster4No, "switch");
            LoadInfo.Save(LoadInfo.booster5No, "meteor");
            GameObject booster1;
            GameObject booster3;
            GameObject booster4;

            booster1 = GameObject.FindGameObjectWithTag("GB1");
            booster3 = GameObject.FindGameObjectWithTag("GB3");
            booster4 = GameObject.FindGameObjectWithTag("GB4");

            if(booster1 != null && booster3 != null && booster4 != null)
            {                
                Booster b1 = booster1.GetComponent<Booster>();
                Booster b3 = booster3.GetComponent<Booster>();
                Booster b4 = booster4.GetComponent<Booster>();
                if (b1 != null && b3 != null && b4 != null)
                {
                    //Debug.Log("my Debug");
                    b1.UpdateGameBoosters();
                    b3.UpdateGameBoosters();
                    b4.UpdateGameBoosters();
                }

            }
            
        }

        else if (String.Equals(args.purchasedProduct.definition.id, pregamePack1, StringComparison.Ordinal))
        {
            //Debug.Log("Pregame Pack 1");
            ++LoadInfo.booster3No;
            ++LoadInfo.booster6No;
            ++LoadInfo.booster7No;
            LoadInfo.Save(LoadInfo.booster3No, "colorBomb");
            LoadInfo.Save(LoadInfo.booster6No, "jumping");
            LoadInfo.Save(LoadInfo.booster7No, "big");
            GameObject window;

            window = GameObject.FindGameObjectWithTag("messageWindow");
            
            if (window != null)
            {
                MessageWindow message = window.GetComponent<MessageWindow>();
                
                if (message != null)
                {
                    message.UpdatePreGameBoosters();
                }
            }           
        }

        else if (String.Equals(args.purchasedProduct.definition.id, allBoostersPack1, StringComparison.Ordinal))
        {
            //Debug.Log("All Boosters Pack 1");
            ++LoadInfo.booster1No;
            ++LoadInfo.booster3No;
            ++LoadInfo.booster4No;
            ++LoadInfo.booster5No;        
            ++LoadInfo.booster6No;
            ++LoadInfo.booster7No;
            LoadInfo.Save(LoadInfo.booster1No, "onePieceRemove");
            LoadInfo.Save(LoadInfo.booster3No, "colorBomb");
            LoadInfo.Save(LoadInfo.booster4No, "switch");
            LoadInfo.Save(LoadInfo.booster5No, "meteor");      
            LoadInfo.Save(LoadInfo.booster6No, "jumping");
            LoadInfo.Save(LoadInfo.booster7No, "big");
            GameObject booster1;
            GameObject booster3;
            GameObject booster4;

            booster1 = GameObject.FindGameObjectWithTag("GB1");
            booster3 = GameObject.FindGameObjectWithTag("GB3");
            booster4 = GameObject.FindGameObjectWithTag("GB4");

            if (booster1 != null && booster3 != null && booster4 != null)
            {
                Booster b1 = booster1.GetComponent<Booster>();
                Booster b3 = booster3.GetComponent<Booster>();
                Booster b4 = booster4.GetComponent<Booster>();
                if (b1 != null && b3 != null && b4 != null)
                {
                    b1.UpdateGameBoosters();
                    b3.UpdateGameBoosters();
                    b4.UpdateGameBoosters();
                }

            }
            GameObject window;

            window = GameObject.FindGameObjectWithTag("messageWindow");

            if (window != null)
            {
                MessageWindow message = window.GetComponent<MessageWindow>();

                if (message != null)
                {
                    message.UpdatePreGameBoosters();
                }
            }
        }

        else if (String.Equals(args.purchasedProduct.definition.id, gamePack3, StringComparison.Ordinal))
        {
            //Debug.Log("Game Pack 3");
            LoadInfo.booster1No = LoadInfo.booster1No + 3;
            LoadInfo.booster4No = LoadInfo.booster4No + 3;
            LoadInfo.booster5No = LoadInfo.booster5No + 3;
            LoadInfo.Save(LoadInfo.booster1No, "onePieceRemove");
            LoadInfo.Save(LoadInfo.booster4No, "switch");
            LoadInfo.Save(LoadInfo.booster5No, "meteor");
            GameObject booster1;
            GameObject booster3;
            GameObject booster4;
            booster1 = GameObject.FindGameObjectWithTag("GB1");
            booster3 = GameObject.FindGameObjectWithTag("GB3");
            booster4 = GameObject.FindGameObjectWithTag("GB4");

            if (booster1 != null && booster3 != null && booster4 != null)
            {
                Booster b1 = booster1.GetComponent<Booster>();
                Booster b3 = booster3.GetComponent<Booster>();
                Booster b4 = booster4.GetComponent<Booster>();
                if (b1 != null && b3 != null && b4 != null)
                {
                    b1.UpdateGameBoosters();
                    b3.UpdateGameBoosters();
                    b4.UpdateGameBoosters();
                }

            }
        }

        else if (String.Equals(args.purchasedProduct.definition.id, pregamePack3, StringComparison.Ordinal))
        {
            //Debug.Log("Pregame Pack 3");
            LoadInfo.booster3No = LoadInfo.booster3No + 3;
            LoadInfo.booster6No = LoadInfo.booster6No + 3;
            LoadInfo.booster7No = LoadInfo.booster7No + 3;
            LoadInfo.Save(LoadInfo.booster3No, "colorBomb");
            LoadInfo.Save(LoadInfo.booster6No, "jumping");
            LoadInfo.Save(LoadInfo.booster7No, "big");
            GameObject window;

            window = GameObject.FindGameObjectWithTag("messageWindow");

            if (window != null)
            {
                MessageWindow message = window.GetComponent<MessageWindow>();

                if (message != null)
                {
                    message.UpdatePreGameBoosters();
                }
            }
        }

        else if (String.Equals(args.purchasedProduct.definition.id, allBoostersPack3, StringComparison.Ordinal))
        {
            //Debug.Log("All Boosters Pack 3");
            LoadInfo.booster1No = LoadInfo.booster1No + 3;
            LoadInfo.booster3No = LoadInfo.booster3No + 3;
            LoadInfo.booster4No = LoadInfo.booster4No + 3;
            LoadInfo.booster5No = LoadInfo.booster5No + 3;          
            LoadInfo.booster6No = LoadInfo.booster6No + 3;
            LoadInfo.booster7No = LoadInfo.booster7No + 3;
            LoadInfo.Save(LoadInfo.booster1No, "onePieceRemove");
            LoadInfo.Save(LoadInfo.booster3No, "colorBomb");
            LoadInfo.Save(LoadInfo.booster4No, "switch");
            LoadInfo.Save(LoadInfo.booster5No, "meteor");
            LoadInfo.Save(LoadInfo.booster6No, "jumping");
            LoadInfo.Save(LoadInfo.booster7No, "big");
            GameObject booster1;
            GameObject booster3;
            GameObject booster4;
            booster1 = GameObject.FindGameObjectWithTag("GB1");
            booster3 = GameObject.FindGameObjectWithTag("GB3");
            booster4 = GameObject.FindGameObjectWithTag("GB4");

            if (booster1 != null && booster3 != null && booster4 != null)
            {
                Booster b1 = booster1.GetComponent<Booster>();
                Booster b3 = booster3.GetComponent<Booster>();
                Booster b4 = booster4.GetComponent<Booster>();
                if (b1 != null && b3 != null && b4 != null)
                {
                    b1.UpdateGameBoosters();
                    b3.UpdateGameBoosters();
                    b4.UpdateGameBoosters();
                }

            }
            GameObject window;

            window = GameObject.FindGameObjectWithTag("messageWindow");

            if (window != null)
            {
                MessageWindow message = window.GetComponent<MessageWindow>();

                if (message != null)
                {
                    message.UpdatePreGameBoosters();
                }
            }
        }

        else if (String.Equals(args.purchasedProduct.definition.id, gamePack5, StringComparison.Ordinal))
        {
            //Debug.Log("Game Pack 5");
            LoadInfo.booster1No = LoadInfo.booster1No + 5;
            LoadInfo.booster4No = LoadInfo.booster4No + 5;
            LoadInfo.booster5No = LoadInfo.booster5No + 5;
            LoadInfo.Save(LoadInfo.booster1No, "onePieceRemove");
            LoadInfo.Save(LoadInfo.booster4No, "switch");
            LoadInfo.Save(LoadInfo.booster5No, "meteor");
            GameObject booster1;
            GameObject booster3;
            GameObject booster4;
            booster1 = GameObject.FindGameObjectWithTag("GB1");
            booster3 = GameObject.FindGameObjectWithTag("GB3");
            booster4 = GameObject.FindGameObjectWithTag("GB4");

            if (booster1 != null && booster3 != null && booster4 != null)
            {
                Booster b1 = booster1.GetComponent<Booster>();
                Booster b3 = booster3.GetComponent<Booster>();
                Booster b4 = booster4.GetComponent<Booster>();
                if (b1 != null && b3 != null && b4 != null)
                {
                    b1.UpdateGameBoosters();
                    b3.UpdateGameBoosters();
                    b4.UpdateGameBoosters();
                }

            }
        }

        else if (String.Equals(args.purchasedProduct.definition.id, pregamePack5, StringComparison.Ordinal))
        {
            //Debug.Log("Pregame Pack 5");
            LoadInfo.booster3No = LoadInfo.booster3No + 5;
            LoadInfo.booster6No = LoadInfo.booster6No + 5;
            LoadInfo.booster7No = LoadInfo.booster7No + 5;
            LoadInfo.Save(LoadInfo.booster3No, "colorBomb");
            LoadInfo.Save(LoadInfo.booster6No, "jumping");
            LoadInfo.Save(LoadInfo.booster7No, "big");
            GameObject window;

            window = GameObject.FindGameObjectWithTag("messageWindow");

            if (window != null)
            {
                MessageWindow message = window.GetComponent<MessageWindow>();

                if (message != null)
                {
                    message.UpdatePreGameBoosters();
                }
            }
        }

        else if (String.Equals(args.purchasedProduct.definition.id, allBoostersPack5, StringComparison.Ordinal))
        {
            //Debug.Log("All Boosters Pack 5");
            LoadInfo.booster1No = LoadInfo.booster1No + 5;
            LoadInfo.booster3No = LoadInfo.booster3No + 5;
            LoadInfo.booster4No = LoadInfo.booster4No + 5;
            LoadInfo.booster5No = LoadInfo.booster5No + 5;
            LoadInfo.booster6No = LoadInfo.booster6No + 5;
            LoadInfo.booster7No = LoadInfo.booster7No + 5;
            LoadInfo.Save(LoadInfo.booster1No, "onePieceRemove");
            LoadInfo.Save(LoadInfo.booster3No, "colorBomb");
            LoadInfo.Save(LoadInfo.booster4No, "switch");
            LoadInfo.Save(LoadInfo.booster5No, "meteor");
            LoadInfo.Save(LoadInfo.booster6No, "jumping");
            LoadInfo.Save(LoadInfo.booster7No, "big");
            GameObject booster1;
            GameObject booster3;
            GameObject booster4;
            booster1 = GameObject.FindGameObjectWithTag("GB1");
            booster3 = GameObject.FindGameObjectWithTag("GB3");
            booster4 = GameObject.FindGameObjectWithTag("GB4");

            if (booster1 != null && booster3 != null && booster4 != null)
            {
                Booster b1 = booster1.GetComponent<Booster>();
                Booster b3 = booster3.GetComponent<Booster>();
                Booster b4 = booster4.GetComponent<Booster>();
                if (b1 != null && b3 != null && b4 != null)
                {
                    b1.UpdateGameBoosters();
                    b3.UpdateGameBoosters();
                    b4.UpdateGameBoosters();
                }

            }
            GameObject window;

            window = GameObject.FindGameObjectWithTag("messageWindow");

            if (window != null)
            {
                MessageWindow message = window.GetComponent<MessageWindow>();

                if (message != null)
                {
                    message.UpdatePreGameBoosters();
                }
            }
        }

        else if (String.Equals(args.purchasedProduct.definition.id, gamePack15, StringComparison.Ordinal))
        {
            //Debug.Log("Game Pack 15");
            LoadInfo.booster1No = LoadInfo.booster1No + 15;
            LoadInfo.booster4No = LoadInfo.booster4No + 15;
            LoadInfo.booster5No = LoadInfo.booster5No + 15;
            LoadInfo.Save(LoadInfo.booster1No, "onePieceRemove");
            LoadInfo.Save(LoadInfo.booster4No, "switch");
            LoadInfo.Save(LoadInfo.booster5No, "meteor");
            GameObject booster1;
            GameObject booster3;
            GameObject booster4;
            booster1 = GameObject.FindGameObjectWithTag("GB1");
            booster3 = GameObject.FindGameObjectWithTag("GB3");
            booster4 = GameObject.FindGameObjectWithTag("GB4");

            if (booster1 != null && booster3 != null && booster4 != null)
            {
                Booster b1 = booster1.GetComponent<Booster>();
                Booster b3 = booster3.GetComponent<Booster>();
                Booster b4 = booster4.GetComponent<Booster>();
                if (b1 != null && b3 != null && b4 != null)
                {
                    b1.UpdateGameBoosters();
                    b3.UpdateGameBoosters();
                    b4.UpdateGameBoosters();
                }

            }
        }

        else if (String.Equals(args.purchasedProduct.definition.id, pregamePack15, StringComparison.Ordinal))
        {
            //Debug.Log("Pregame Pack 15");
            LoadInfo.booster3No = LoadInfo.booster3No + 15;
            LoadInfo.booster6No = LoadInfo.booster6No + 15;
            LoadInfo.booster7No = LoadInfo.booster7No + 15;
            LoadInfo.Save(LoadInfo.booster3No, "colorBomb");
            LoadInfo.Save(LoadInfo.booster6No, "jumping");
            LoadInfo.Save(LoadInfo.booster7No, "big");
            GameObject window;

            window = GameObject.FindGameObjectWithTag("messageWindow");

            if (window != null)
            {
                MessageWindow message = window.GetComponent<MessageWindow>();

                if (message != null)
                {
                    message.UpdatePreGameBoosters();
                }
            }
        }

        else if (String.Equals(args.purchasedProduct.definition.id, allBoostersPack15, StringComparison.Ordinal))
        {
            //Debug.Log("All Boosters Pack 15");
            LoadInfo.booster1No = LoadInfo.booster1No + 15;
            LoadInfo.booster3No = LoadInfo.booster3No + 15;
            LoadInfo.booster4No = LoadInfo.booster4No + 15;
            LoadInfo.booster5No = LoadInfo.booster5No + 15;
            LoadInfo.booster6No = LoadInfo.booster6No + 15;
            LoadInfo.booster7No = LoadInfo.booster7No + 15;
            LoadInfo.Save(LoadInfo.booster1No, "onePieceRemove");
            LoadInfo.Save(LoadInfo.booster3No, "colorBomb");
            LoadInfo.Save(LoadInfo.booster4No, "switch");
            LoadInfo.Save(LoadInfo.booster5No, "meteor");
            LoadInfo.Save(LoadInfo.booster6No, "jumping");
            LoadInfo.Save(LoadInfo.booster7No, "big");
            GameObject booster1;
            GameObject booster3;
            GameObject booster4;
            booster1 = GameObject.FindGameObjectWithTag("GB1");
            booster3 = GameObject.FindGameObjectWithTag("GB3");
            booster4 = GameObject.FindGameObjectWithTag("GB4");

            if (booster1 != null && booster3 != null && booster4 != null)
            {
                Booster b1 = booster1.GetComponent<Booster>();
                Booster b3 = booster3.GetComponent<Booster>();
                Booster b4 = booster4.GetComponent<Booster>();
                if (b1 != null && b3 != null && b4 != null)
                {
                    b1.UpdateGameBoosters();
                    b3.UpdateGameBoosters();
                    b4.UpdateGameBoosters();
                }

            }
            GameObject window;

            window = GameObject.FindGameObjectWithTag("messageWindow");

            if (window != null)
            {
                MessageWindow message = window.GetComponent<MessageWindow>();

                if (message != null)
                {
                    message.UpdatePreGameBoosters();
                }
            }
        }

        else
        {
            //Debug.Log("Purchase Failed");
        }
        return PurchaseProcessingResult.Complete;
    }










    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                //Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                //Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            //Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            //Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            //Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                //Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            //Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        //Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        //Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}