using UnityEngine;
using System.Collections;
using OnePF;
using System;
using System.Collections.Generic;

public class IAPManager : MonoBehaviour {
	
	public Action<string> result = delegate (string str) {};
	public Action<string> purchaseSucceeded = delegate (string sku) {};
	public Action restoreSucceeded = delegate () {};
	public Action<Inventory> queryInventorySucceeded = delegate (Inventory inventory) {};
	
	string _label {
		set{ result(value);}
	}
	public bool _isInitialized = false;
	public Inventory _inventory = null;
	
	const string SKU = "Global-Top Killers";
	
	private void OnEnable()
	{
		// Listen to all events for illustration purposes
		OpenIABEventManager.billingSupportedEvent += billingSupportedEvent;
		OpenIABEventManager.billingNotSupportedEvent += billingNotSupportedEvent;
		OpenIABEventManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		OpenIABEventManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		OpenIABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
		OpenIABEventManager.purchaseFailedEvent += purchaseFailedEvent;
		OpenIABEventManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		OpenIABEventManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
		OpenIABEventManager.restoreSucceededEvent += restoreSucceededEvent;
	}
	private void OnDisable()
	{
		// Remove all event handlers
		OpenIABEventManager.billingSupportedEvent -= billingSupportedEvent;
		OpenIABEventManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		OpenIABEventManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		OpenIABEventManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		OpenIABEventManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		OpenIABEventManager.purchaseFailedEvent -= purchaseFailedEvent;
		OpenIABEventManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		OpenIABEventManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
		OpenIABEventManager.restoreSucceededEvent -= restoreSucceededEvent;
		
	}
	
	public static IAPManager shared;
	
	void Awake(){
		
		if (shared != null) {
			DestroyImmediate (gameObject);
			return;
		}
		
		shared = this;
		DontDestroyOnLoad(gameObject);
		
		
		
	}
	
	private void Start()
	{
		
		OpenIAB.mapSku(SKU, OpenIAB_Android.STORE_GOOGLE, "pack.topglobal");
		OpenIAB.mapSku(SKU, OpenIAB_iOS.STORE, "pack.topglobal");
		
		InitOpenIAB ();
	}
	
	void InitOpenIAB()
	{
		var googlePublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjGg+p9dYT3X41S8CwJCsdilPjhiLzvdVL2CBT/APh3Nkk4fiFs6cpqecgl+H9h9SmbPhWJ8qrWDiTTV1bLKtlEI4lQkCJ5U8nT13Z6U7VmP7h1A+DHyvEvvF3ox7kZmY4x9LCypHav/7DsS88eWA0zLFm8+juqBprPsGupy6JasSibvC0MWWIcvl3vJ5LCQKppP/1BJV+IE7Y8s0NQFwI2ZcgprFBsqQr/h2r9+zFAcS7D0WtiJq/jS/PJqav/6bXo5UWVXmZCfC6GavucIKybOdybNbb/K0MXqph6MkPaqgYT+If9h/Vlt8tIafWE6SuKtFMk0I8XbqdAfpFbgkzQIDAQAB/uhGQaCoZO/I+fhgpfiwKkDRf+STIWSvJyuSK3GipO7EnADSlAIa1l+dGGL+7fsAuEV+sp0cQOfWSsELB9t9TnEsA3g9lHz4z7EsdiyPl/GOr0ei3KHdGM3t20pqVE/hy48de7daiiKbgg9po99J8bVcZktLwbqc12H1WsCjDYsJjRMz5uDGttFHv1/xojkbqgbsolJK3GcaUhj0z6tkmRxZo64z9LI1TxjaDt3p+mDQxTFBpY0V1mb1BcSAY9ko3rEJes37TEi7HNZCvzrRZGBTfKB62OsQIDAQAB";
		
		var options = new Options();
		options.storeKeys = new Dictionary<string, string> { {OpenIAB_Android.STORE_GOOGLE, googlePublicKey} };
		//options.checkInventoryTimeoutMs = Options.INVENTORY_CHECK_TIMEOUT_MS * 2;
		//options.discoveryTimeoutMs = Options.DISCOVER_TIMEOUT_MS * 2;
		//options.checkInventory = false;
		options.verifyMode = OptionsVerifyMode.VERIFY_SKIP;
		options.storeSearchStrategy = SearchStrategy.INSTALLER_THEN_BEST_FIT;
		
		OpenIAB.init(options);
	}
	
	public void Buy(string sku = SKU)
	{
		print ("Buying - "+sku);
		OpenIAB.purchaseProduct (sku);
	}
	
	public bool HasProduct(string sku)
	{
		return _inventory != null && _inventory.HasPurchase (sku);
	}

	public void Restore()
	{
		OpenIAB.restoreTransactions ();
	}
	
	private void billingSupportedEvent()
	{
		_isInitialized = true;
		OpenIAB.queryInventory ();
		
		_label = "billingSupportedEvent";
		Debug.Log("billingSupportedEvent");
	}
	private void billingNotSupportedEvent(string error)
	{
		Debug.Log("billingNotSupportedEvent: " + error);
	}
	private void queryInventorySucceededEvent(Inventory inventory)
	{
		Debug.Log("queryInventorySucceededEvent: " + inventory);
		/*if (inventory != null)
		{
			_label = inventory.ToString();
		}*/
		_inventory = inventory;
		print (inventory.ToString());
		queryInventorySucceeded(inventory);
	}
	private void queryInventoryFailedEvent(string error)
	{
		Debug.Log("queryInventoryFailedEvent: " + error);
		_label = error;
	}
	private void purchaseSucceededEvent(Purchase purchase)
	{
		Debug.Log("purchaseSucceededEvent: " + purchase);
		_label = "PURCHASED:" + purchase.ToString();

		if (_inventory != null) {
			var sk = _inventory.GetSkuDetails(purchase.Sku);
			var pr = 0f;
			float.TryParse(sk.PriceValue,out pr);
			pr*=100;
			AnalyticsManager.TrackBusinessEvent(sk.CurrencyCode,(int)pr,"pack",sk.Title,"shop");
			Debug.Log("Purchase analytics sent");
		}
		purchaseSucceeded (purchase.Sku);
		OpenIAB.queryInventory ();
	}
	private void purchaseFailedEvent(int errorCode, string errorMessage)
	{
		Debug.Log("purchaseFailedEvent: " + errorMessage);
		_label = "Purchase Failed: " + errorMessage;
	}
	private void consumePurchaseSucceededEvent(Purchase purchase)
	{
		Debug.Log("consumePurchaseSucceededEvent: " + purchase);
		_label = "CONSUMED: " + purchase.ToString();
		
	}
	private void consumePurchaseFailedEvent(string error)
	{
		Debug.Log("consumePurchaseFailedEvent: " + error);
		_label = "Consume Failed: " + error;
	}

	private void restoreSucceededEvent()
	{
		restoreSucceeded ();
		OpenIAB.queryInventory ();
	}
	
}
