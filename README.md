# Sitecore.Commerce.CartMaintenance
Example for creating a minion

1) Add Feature.Carts.Engine project as project reference to your Sitecore.Commerce.Engine project (you might want to deploy )
2) Add file ..\Sitecore.Commerce.CartMaintenance\src\Project\Habitat\Engine\wwwroot\data\Environments\TestMinion.json to the corresponding folder in your Sitecore.Commerce.Engine project
3) Open the config.json at the root of your Sitecore.Commerce.Engine project and change "EnvironmentName" value from "HabitatMinions" to "TestMinion"
NOTE: This makes the engine role start the minions configured within the TestMinion Environment
4) Execute the bootstrapping process from Postman to ingest the TestMinion.json into the database.
5) Start the engine role where you've been making changes


/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) * FROM [sc902_Commerce.SharedEnvironments].[dbo].[CommerceEntities] where id = 'test-cart-to-delete'
SELECT TOP (1000) * FROM [sc902_Commerce.SharedEnvironments].[dbo].[CartsLists]

INSERT INTO [sc902_Commerce.SharedEnvironments].[dbo].[CartsLists] (ListName, EnvironmentId, CommerceEntityId, EntityVersion)
VALUES (
	'List-carts-ByDate',
	'78A1EA61-1F37-42A7-AC89-9A3F46D60CA5',
	'test-cart-to-delete',
	1
)

INSERT INTO [sc902_Commerce.SharedEnvironments].[dbo].[CommerceEntities] (Id, EnvironmentId, version, Entity,EntityVersion,Published)
VALUES (
	'test-cart-to-delete', 
	'78A1EA61-1F37-42A7-AC89-9A3F46D60CA5', 
	7, 
	'{"$type":"Sitecore.Commerce.Plugin.Carts.Cart, Sitecore.Commerce.Plugin.Carts","ShopName":"Storefront","ItemCount":0,"Lines":{"$type":"System.Collections.Generic.List`1[[Sitecore.Commerce.Plugin.Carts.CartLineComponent, Sitecore.Commerce.Plugin.Carts]], mscorlib","$values":[]},"Totals":{"$type":"Sitecore.Commerce.Plugin.Carts.Totals, Sitecore.Commerce.Plugin.Carts","SubTotal":{"$type":"Sitecore.Commerce.Core.Money, Sitecore.Commerce.Core","CurrencyCode":"USD","Amount":0.0},"AdjustmentsTotal":{"$type":"Sitecore.Commerce.Core.Money, Sitecore.Commerce.Core","CurrencyCode":"USD","Amount":0.0},"GrandTotal":{"$type":"Sitecore.Commerce.Core.Money, Sitecore.Commerce.Core","CurrencyCode":"USD","Amount":0.0},"PaymentsTotal":{"$type":"Sitecore.Commerce.Core.Money, Sitecore.Commerce.Core","CurrencyCode":"USD","Amount":0.0},"Name":"","Policies":{"$type":"System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Policy, Sitecore.Commerce.Core]], mscorlib","$values":[]}},"Adjustments":{"$type":"System.Collections.Generic.List`1[[Sitecore.Commerce.Plugin.Pricing.AwardedAdjustment, Sitecore.Commerce.Plugin.Pricing]], mscorlib","$values":[]},"Components":{"$type":"System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Component, Sitecore.Commerce.Core]], mscorlib","$values":[{"$type":"Sitecore.Commerce.Plugin.ManagedLists.ListMembershipsComponent, Sitecore.Commerce.Plugin.ManagedLists","Memberships":{"$type":"System.Collections.Generic.List`1[[System.String, mscorlib]], mscorlib","$values":["Carts"]},"Id":"2078abcff4154153ab9e5c06235526c2","Name":"","Comments":"","Policies":{"$type":"System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Policy, Sitecore.Commerce.Core]], mscorlib","$values":[]},"ChildComponents":{"$type":"System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Component, Sitecore.Commerce.Core]], mscorlib","$values":[]}}]},"DateCreated":"2018-09-09T06:05:43.5678598+00:00","DateUpdated":"2018-01-01T05:02:25.3102763+00:00","DisplayName":"","Id":"test-cart-to-delete","Version":7,"EntityVersion":1,"Published":true,"IsPersisted":true,"Name":"test-cart-to-delete","Policies":{"$type":"System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Policy, Sitecore.Commerce.Core]], mscorlib","$values":[]}}',
	1,
	1
)