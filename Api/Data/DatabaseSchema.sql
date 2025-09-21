CREATE TABLE "TaxRate" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_TaxRate" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Rate" REAL NOT NULL,
    "EffectiveFrom" TEXT NOT NULL,
    "EffectiveTo" TEXT NULL
);



CREATE TABLE "Message" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Message" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Subject" TEXT NOT NULL,
    "Content" TEXT NOT NULL,
    "Processed" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL
);



CREATE TABLE "User" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY,
    "Username" TEXT NOT NULL,
    "PasswordHash" TEXT NOT NULL,
    "IsAdmin" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL
);
CREATE UNIQUE INDEX "UsernameIndex" ON "User" ("Username");



CREATE TABLE "Payment" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Payment" PRIMARY KEY AUTOINCREMENT,
    "Amount" REAL NOT NULL,
    "PaymentMethod" TEXT NOT NULL,
    "CardName" TEXT NOT NULL,
    "CardNumber" TEXT NOT NULL,
    "Expiry" TEXT NOT NULL,
    "CVV" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL
);



CREATE TABLE "Address" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Address" PRIMARY KEY AUTOINCREMENT,
    "AddressLineOne" TEXT NOT NULL,
    "AddressLineTwo" TEXT NULL,
    "Town" TEXT NOT NULL,
    "County" TEXT NULL,
    "PostCode" TEXT NOT NULL,
    "Country" TEXT NOT NULL
);



CREATE TABLE "Customer" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Customer" PRIMARY KEY AUTOINCREMENT,
    "Email" TEXT NOT NULL,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "PhoneNumber" TEXT NULL,
    "BillingAddressId" INTEGER NULL,
    "ShippingAddressId" INTEGER NULL,
    "UserId" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Customer_Address_BillingAddressId" FOREIGN KEY ("BillingAddressId") REFERENCES "Address" ("Id"),
    CONSTRAINT "FK_Customer_Address_ShippingAddressId" FOREIGN KEY ("ShippingAddressId") REFERENCES "Address" ("Id"),
    CONSTRAINT "FK_Customer_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_Customer_BillingAddressId" ON "Customer" ("BillingAddressId");
CREATE INDEX "IX_Customer_ShippingAddressId" ON "Customer" ("ShippingAddressId");
CREATE INDEX "IX_Customer_UserId" ON "Customer" ("UserId");
CREATE UNIQUE INDEX "EmailIndex" ON "Customer" ("Email");



CREATE TABLE "Category" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Category" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL
);



CREATE TABLE "Product" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Product" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "Price" REAL NOT NULL,
    "ImageURL" TEXT NOT NULL,
    "Stock" INTEGER NOT NULL,
    "ForSale" INTEGER NOT NULL,
    "SalePrice" REAL NULL,
    "CategoryId" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Product_Category_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Category" ("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_Product_CategoryId" ON "Product" ("CategoryId");



CREATE TABLE "ProductAttribute" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_ProductAttribute" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Value" TEXT NOT NULL,
    "ProductId" INTEGER NULL,
    CONSTRAINT "FK_ProductAttribute_Product_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Product" ("Id")
);
CREATE INDEX "IX_ProductAttribute_ProductId" ON "ProductAttribute" ("ProductId");



CREATE TABLE "Basket" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Basket" PRIMARY KEY AUTOINCREMENT,
    "AnonymousId" TEXT NULL,
    "CustomerId" INTEGER NULL,
    "CreatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Basket_Customer_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customer" ("Id")
);
CREATE INDEX "IX_Basket_CustomerId" ON "Basket" ("CustomerId");



CREATE TABLE "BasketItem" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_BasketItem" PRIMARY KEY AUTOINCREMENT,
    "BasketId" INTEGER NOT NULL,
    "ProductId" INTEGER NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "Price" REAL NOT NULL,
    "VATRate" REAL NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_BasketItem_Basket_BasketId" FOREIGN KEY ("BasketId") REFERENCES "Basket" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BasketItem_Product_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Product" ("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_BasketItem_BasketId" ON "BasketItem" ("BasketId");
CREATE INDEX "IX_BasketItem_ProductId" ON "BasketItem" ("ProductId");



CREATE TABLE "Order" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Order" PRIMARY KEY AUTOINCREMENT,
    "OrderNumber" TEXT NOT NULL,
    "Status" TEXT NOT NULL,
    "TotalPrice" REAL NOT NULL,
    "VATRate" REAL NOT NULL,
    "ContactPhoneNumber" TEXT NOT NULL DEFAULT '',
    "DeliveryMethod" TEXT NOT NULL,
    "EstimatedDelivery" TEXT NULL,
    "CustomerId" INTEGER NOT NULL,
    "PaymentId" INTEGER NOT NULL,
    "BillingAddressId" INTEGER NOT NULL,
    "ShippingAddressId" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Order_Address_BillingAddressId" FOREIGN KEY ("BillingAddressId") REFERENCES "Address" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Order_Address_ShippingAddressId" FOREIGN KEY ("ShippingAddressId") REFERENCES "Address" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Order_Customer_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customer" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Order_Payment_PaymentId" FOREIGN KEY ("PaymentId") REFERENCES "Payment" ("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_Order_CustomerId" ON "Order" ("CustomerId");
CREATE INDEX "IX_Order_PaymentId" ON "Order" ("PaymentId");
CREATE INDEX "IX_Order_BillingAddressId" ON "Order" ("BillingAddressId");
CREATE INDEX "IX_Order_ShippingAddressId" ON "Order" ("ShippingAddressId");



CREATE TABLE "OrderItem" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItem" PRIMARY KEY AUTOINCREMENT,
    "Quantity" INTEGER NOT NULL,
    "UnitPrice" REAL NOT NULL,
    "TotalPrice" REAL NOT NULL,
    "OrderId" INTEGER NOT NULL,
    "ProductId" INTEGER NOT NULL,
    "VATRate" REAL NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_OrderItem_Order_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Order" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_OrderItem_Product_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Product" ("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_OrderItem_OrderId" ON "OrderItem" ("OrderId");
CREATE INDEX "IX_OrderItem_ProductId" ON "OrderItem" ("ProductId");



CREATE TABLE "OrderTrackingUpdate" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderTrackingUpdate" PRIMARY KEY AUTOINCREMENT,
    "Status" TEXT NOT NULL,
    "Note" TEXT NOT NULL,
    "OrderId" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedBy" TEXT NOT NULL,
    CONSTRAINT "FK_OrderTrackingUpdate_Order_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Order" ("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_OrderTrackingUpdate_OrderId" ON "OrderTrackingUpdate" ("OrderId");



CREATE TABLE "Review" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Review" PRIMARY KEY AUTOINCREMENT,
    "Subject" TEXT NOT NULL,
    "Rating" INTEGER NOT NULL,
    "Comment" TEXT NOT NULL,
    "ProductId" INTEGER NOT NULL,
    "CustomerId" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL, "Status" TEXT NOT NULL DEFAULT '',
    CONSTRAINT "FK_Review_Customer_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customer" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Review_Product_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Product" ("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_Review_CustomerId" ON "Review" ("CustomerId");
CREATE INDEX "IX_Review_ProductId" ON "Review" ("ProductId");



CREATE TABLE "Wishlist" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Wishlist" PRIMARY KEY AUTOINCREMENT,
    "CustomerId" INTEGER NOT NULL,
    "ProductId" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Wishlist_Customer_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customer" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Wishlist_Product_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Product" ("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_Wishlist_CustomerId" ON "Wishlist" ("CustomerId");
CREATE INDEX "IX_Wishlist_ProductId" ON "Wishlist" ("ProductId");