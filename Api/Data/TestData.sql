INSERT INTO TaxRate (Id, [Name], Rate, EffectiveFrom, EffectiveTo)
SELECT 1, 'VAT (20%)', 0.2, '2024-09-16 10:48:14.2885919', NULL
GO;

INSERT INTO Category (Id, [Name], [Description], CreatedAt)
SELECT 1, 'Automotive', 'Explore different types of mobility products to help develop your car and make it feel more comforting. There are car accessories and things like replacement parts. It helps DIY and mechanics to explore a wide range of new products to use to develop the car/bike you are working on.', '2024-09-14 22:40:39.7732556'
UNION SELECT 2, 'Books', 'Find all types of books with different genres to educational, fiction and non – fiction. Even if you just read for fun or if you want to get into reading there is a wide variety of different books to help you improve.', '2024-09-14 22:40:39.7732556'
UNION SELECT 3, 'Clothing', 'Here you can find different types of clothing for all ages. You can find everyday wear or even normal outfits. These types of clothes are affordable and available to mostly everyone. There is clothing for every type of season whether it is winter or summer.', '2024-09-14 22:40:39.7732556'
UNION SELECT 4, 'Electronics', 'Here you can find the best rated electronics from mobile phones to TVs and cameras. You can stay up to date with the latest trends and find devise that benefit you. In addition, headphones for the gym.', '2024-09-14 22:40:39.7732556'
UNION SELECT 5, 'Sports & Outdoors', 'Find different types of gear for outdoors to help you stay more active like hiking equipment to footballs and tennis racquets. There is fitness gear and different accessories to help support your active lifestyle.', '2024-09-14 22:40:39.7732556'
UNION SELECT 6, 'Toys & Games', 'Find different types of fun for all ages with a variety of different toys and games to help keep you entertained. From toys for children to creative games where you can learn new things or even drive your favourite car.', '2024-09-14 22:40:39.7732556'
GO;

INSERT INTO Address (Id, AddressLineOne, AddressLineTwo, Town, County, PostCode, Country)
SELECT 1, 'Downing Street', 'Apartment 10', 'London', 'Greater London', 'SW1A 2AA', 'UK'
UNION SELECT 2, '221B Baker Street', 'Flat 4', 'London', 'Greater London', 'NW1 6XE', 'UK'
UNION SELECT 3, '1 Castle Street', 'Floor 3', 'Edinburgh', 'Midlothian', 'EH1 2ND', 'UK'
UNION SELECT 4, '50 George Square', 'Flat 2', 'Edinburgh', 'Midlothian', 'EH8 9JU', 'UK'
UNION SELECT 5, '100 King Street', 'Penthouse', 'Manchester', 'Greater Manchester', 'M2 4WU', 'UK'
UNION SELECT 6, '200 Deansgate', 'Apt 5B', 'Manchester', 'Greater Manchester', 'M3 3NN', 'UK'
UNION SELECT 7, '1 Queen Square', 'Ground Floor', 'Bristol', 'Bristol', 'BS1 4JQ', 'UK'
UNION SELECT 8, '15 Park Street', 'Flat 5b', 'Bristol', 'Greater Bristol', 'BS1 5HX', 'UK'
UNION SELECT 9, '30 Bold Street', 'Flat 1a', 'Liverpool', 'Merseyside', 'L1 4DS', 'UK'
UNION SELECT 10, '8 Hope Street', 'Apartment 9c', 'Liverpool', 'Merseyside', 'L1 9BX', 'UK'
UNION SELECT 11, '10 High Street', 'Apartment 7D', 'Rochdale', 'Greater Manchester', 'OL11 5PZ', 'UK'
UNION SELECT 12, '11 Lower Road', 'Apartment 7D', 'Rochdale', 'Greater Manchester', 'OL11 5PZ', 'UK'
GO;

INSERT INTO [User] (Id, Username, PasswordHash, IsAdmin, CreatedAt)
SELECT 1, 'jdoe@example.com', 'ENBzzGQwXA4CDuUGn212f3t251Wo9j+7461u56Vo7l0=', 0, '2024-09-14 22:40:39.7732556'
UNION SELECT 2, 'asmith@example.com', 'ENBzzGQwXA4CDuUGn212f3t251Wo9j+7461u56Vo7l0=', 0, '2024-09-14 22:40:39.7732556'
UNION SELECT 3, 'bwayne@example.com', 'ENBzzGQwXA4CDuUGn212f3t251Wo9j+7461u56Vo7l0=', 0, '2024-09-14 22:40:39.7732556'
UNION SELECT 4, 'ckent@example.com', 'ENBzzGQwXA4CDuUGn212f3t251Wo9j+7461u56Vo7l0=', 0, '2024-09-14 22:40:39.7732556'
UNION SELECT 5, 'dprince@example.com', 'ENBzzGQwXA4CDuUGn212f3t251Wo9j+7461u56Vo7l0=', 0, '2024-09-14 22:40:39.7732556'
UNION SELECT 6, 'admin@bobkart.com', 'ENBzzGQwXA4CDuUGn212f3t251Wo9j+7461u56Vo7l0=', 1, '2024-09-14 22:40:39.7732556'
GO;

INSERT INTO Customer (Id, FirstName, LastName, Email, PhoneNumber, BillingAddressId, ShippingAddressId, UserId, CreatedAt)
SELECT 1, 'John', 'Doe', 'jdoe@example.com', '07123456789', 1, 2, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 2, 'Alice', 'Smith', 'asmith@example.com', '07234567890', 3, 4, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 3, 'Bruce', 'Wayne', 'bwayne@example.com', '07345678901', 5, 6, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 4, 'Clark', 'Kent', 'ckent@example.com', '07456789012', 7, 8, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 5, 'Diana', 'Prince', 'dprince@example.com', '07567890123', 9, 10, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 6, 'Bob', 'Kart', 'admin@bobkart.com', '07111222333', 11, 12, 6, '2024-09-14 22:40:39.7732556'
GO;

INSERT INTO Product (Id, [Name], [Description], Price, ImageURL, Stock, ForSale, SalePrice, CategoryId, CreatedAt)
SELECT 1, 'Car Vacuum', 'Car Vacuum Portable Cordless, Handheld Cordless with High Power, Type-C Charging, 2 Suction Modes, Up to 40 Mins Runtime, Small Mini Vacuum for Car, Pet, Home. This practical car vacuum comes with 5 accessories. The accessories can be used individually or combined, making it perfect for interior car cleaning. As a cordless handheld vacuum, it effortlessly handles tasks like inflating air cushions or swimming rings.', 29.99, '/images/products/car vaccum.webp', 50, 1, 24.99, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 2, 'Dash Cam', 'Dash Cam Front Rear Dashcam - Car Camera Dual Lens Dashcams for Cars with 32GB Card Support 170° Wide Angle 1080P Full HD G-Sensor Loop Recording 24H Parking Mode Ginarelo.  Our dashcam is equipped with 1080P FHD camera, and the dash cam front supports 170 ° wide-angle shooting, easily covers multiple lanes and reduces visual blind spots. The wireless dash cams for cars is equipped with a high-sensitivity G-sensor. When the vehicle collides or brakes suddenly, the device will instantly lock the accident scene.', 59.99, '/images/products/dash cam.webp', 35, 0, NULL, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 3, 'Car Cover', 'Car Cover for SUV Full Size | Heavy Duty Outdoor Waterproof & Breathable | UV & Snow Protection | Fits SUVs. All-Season Protection – Shields your SUV from rain, snow, UV rays, dust, and bird droppings year-round. Durable 4-Layer Material – Thick, strong fabric ensures long-lasting and reliable vehicle protection.', 49.99, '/images/products/car cover.webp', 25, 1, 39.99, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 4, 'Tire Inflator', 'Tyre Inflator Portable Air Compressor, 5 Mode Cordless Tyre Inflator 150PSI 6000mAh Rechargeable Power Bank Auto Shut-Off Digital Display Electric Air Pump w LED Light for Bike, Ball, Car, Motorcycle. Dual-screen Digital, Accurate Display and is cordless making it easy to use.', 34.99, '/images/products/tire inflator.webp', 40, 1, 27.99, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 5, 'Seat Organizer', 'Car Seat Organiser 2 Pcs Back Seat Protector for Kids, Car Organiser Back Seat for up to 10 iPad, Kids Back Seat Organiser with 5 Pockets to Storage Toys, Books, Drinks. There are multi-Pocket Storage with this baby car seat organisers. Premium quality better than others, which is environmentally friendly materials 600D Polyester.', 19.99, '/images/products/seat organizer.webp', 60, 0, NULL, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 6, 'Steering Wheel Cover', 'Car Steering Wheel Cover Leather - Soft Microfiber Steering Wheel Cover Universal Size M 37-38cm /14.5-15inch, Anti-slip, Breathable, Black. This Sportage car steering wheel cover made of soft microfiber leather. We promise to providing a 12-month warranty on this car steering wheel cover leather for man quality-related issues.', 15.99, '/images/products/steering wheel cover.webp', 75, 1, 12.99, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 7, 'Car Air Freshener', 'Air Re-Fresher Odor Fighting Spray 237ml - New Car Scent - Convenient Car Air Freshener that Instantly Fights Odors and Leaves Behind a Long-Lasting New Car Scent. Simply mist over the affected carpet and allow the product to dry. Perfect for fighting odors interiors of cars, trucks, SUVs, RVs & Boats.', 7.99, '/images/products/car air freshner.webp', 100, 0, NULL, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 8, 'Jump Starter', 'NOCO Boost GB70: 2000A Ultra Safe Jump Starter Power Pack – 12V Car Battery Booster, Portable Power Bank & Jump Leads - For 8.0L Petrol and 6.0L Diesel Engines. instantly start dead batteries with 2000 amps of peak lithium power. Up to 40 starts per charge on engines up to 8.0L petrol and 6.0L diesel. Engineered with patented spark-proof.', 69.99, '/images/products/jump starter.webp', 20, 1, 59.99, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 9, 'Windshield Sun Shade', 'Universal Car Sun Shade for Windshield – Foldable Foil Reflector Cover for UV & Heat Protection, Sunshade for Cars, Vans, SUVs, Front or Rear Window Sun Blocker. Magnetic Protection: Built-in magnets along the edges secure the cover firmly, ensuring it stays in place even in windy conditions. Blocks UV & Heat: Reflective foil design effectively blocks harmful UV rays. Lightweight and foldable for easy storage.', 12.99, '/images/products/windshield sun shade.webp', 45, 0, NULL, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 10, 'LED Headlights', 'GSRECY H4 LED Headlight Bulb for Car, 80W 12000LM H4 Vehicle LED Headlights Conversion Kit H4 9003 HB2 Headlight Bulb Super Bright 6000K White, Plug and Play, Pack of 2. SRECY LED headlight bulb with ZES-3570 automotive light source LED Chips. It has a quick direct plug ins making it easy to install.', 39.99, '/images/products/led headlights.webp', 30, 1, 34.99, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 11, 'The Great Gatsby', 'Discover F. Scott Fitzgeralds masterpiece that captures the essence of the Jazz Age. This timeless classic explores themes of love, wealth, and the American Dream through the eyes of Nick Carraway as he becomes entangled in the world of the mysterious Jay Gatsby. A profound commentary on the decadence and idealism of the 1920s.', 12.99, '/images/products/the great gatsby.webp', 85, 1, 9.99, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 12, 'To Kill a Mockingbird', 'Harper Lees Pulitzer Prize-winning novel that addresses racial inequality and moral growth in the American South. Through the eyes of young Scout Finch, readers experience the profound impact of her father Atticuss defense of a black man wrongly accused of rape. A powerful exploration of justice and childhood innocence.', 14.99, '/images/products/to kill a mockingbird.webp', 75, 0, NULL, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 13, '1984', 'George Orwells dystopian masterpiece that paints a chilling picture of a totalitarian future where critical thought is suppressed under the watchful eye of Big Brother. This profound exploration of surveillance, propaganda, and individual freedom remains remarkably relevant in todays digital age.', 11.99, '/images/products/1984.webp', 65, 1, 8.99, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 14, 'Pride and Prejudice', 'Jane Austens beloved romantic novel that follows the spirited Elizabeth Bennet as she navigates the social expectations and romantic entanglements of 19th-century England. Her tumultuous relationship with the proud Mr. Darcy explores themes of personal growth, first impressions, and true love.', 10.99, '/images/products/pride and prejudice.webp', 90, 1, 8.49, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 15, 'The Hobbit', 'J.R.R. Tolkiens enchanting prelude to The Lord of the Rings. Join Bilbo Baggins, a comfort-loving hobbit, on an unexpected adventure with a company of dwarves to reclaim their mountain home from the dragon Smaug. A timeless tale of courage, friendship, and discovering ones own strength.', 13.5, '/images/products/the hobbit.gif', 80, 0, NULL, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 16, 'Harry Potter and the Philosophers Stone', 'The book that started a global phenomenon. Follow young Harry Potter as he discovers he is a wizard and begins his journey at Hogwarts School of Witchcraft and Wizardry. He makes new friends, learns magic, and uncovers the mysterious secret of the Philosophers Stone.', 14.99, '/images/products/harry potter and the philosophers stone.webp', 120, 1, 12.99, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 17, 'The Catcher in the Rye', 'J.D. Salingers influential novel narrated by the disillusioned teenager Holden Caulfield. After being expelled from prep school, he wanders New York City for three days, grappling with the phoniness of the adult world and his own impending transition into it.', 11.25, '/images/products/the catch in the rye.webp', 55, 0, NULL, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 18, 'Lord of the Flies', 'William Goldings chilling allegory about a group of British boys stranded on a deserted island. Their attempt to govern themselves descends into brutal savagery, exploring the dark side of human nature and the thin veneer of civilization.', 9.99, '/images/products/lord of the flies.webp', 70, 1, 7.99, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 19, 'The Alchemist', 'Paulo Coelhos inspirational fable about Santiago, an Andalusian shepherd boy who dreams of finding a worldly treasure. His journey teaches him to listen to his heart and follow his dreams, revealing that the real treasure is the wisdom gained along the way.', 15.99, '/images/products/the alchemist.webp', 95, 0, NULL, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 20, 'The Da Vinci Code', 'Dan Browns international thriller that follows symbologist Robert Langdon and cryptologist Sophie Neveu as they investigate a murder in the Louvre Museum. They become embroiled in a battle between secret societies over a mysterious secret protected for centuries.', 16.5, '/images/products/the da vinci code.webp', 60, 1, 13.99, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 21, 'Classic Cotton T-Shirt', 'Our essential crewneck t-shirt is crafted from 100% premium combed cotton for exceptional softness and durability. Perfect for everyday wear, it features a classic fit, ribbed neckband, and double-stitched sleeves for lasting shape. An versatile staple for any casual wardrobe.', 24.99, '/images/products/cotton t shirt.webp', 200, 1, 19.99, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 22, 'Premium Denim Jeans', 'Experience superior comfort and style with our premium denim jeans. Made from high-quality stretch denim, they offer a modern slim fit that flatters your silhouette. Features include a five-pocket design, durable metal hardware, and a comfortable mid-rise waist for all-day wear.', 89.99, '/images/products/denim jeans.webp', 75, 0, NULL, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 23, 'Soft Fleece Hoodie', 'Stay cozy and stylish in our ultra-soft fleece hoodie. Designed for maximum comfort, it features a spacious front pocket, adjustable drawstring hood, and ribbed cuffs and waistband to seal in warmth. Perfect for cool evenings, lounging, or casual outings.', 59.99, '/images/products/fleece hoodie.webp', 120, 1, 49.99, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 24, 'Waterproof Jacket', 'Tackle the elements with confidence in our fully waterproof and windproof jacket. Constructed with a seam-sealed membrane, it offers complete protection from rain and wind while remaining breathable. Features include an adjustable hood, zippered pockets, and elasticated cuffs.', 129.99, '/images/products/waterproof jacket.webp', 45, 0, NULL, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 25, 'Wool Blend Sweater', 'A timeless addition to your cold-weather wardrobe, this sweater is knitted from a soft wool and acrylic blend for warmth without itchiness. It features a classic cable knit pattern, a ribbed crewneck, and a relaxed fit for easy layering over shirts.', 79.99, '/images/products/wool sweater.webp', 60, 1, 64.99, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 26, 'Performance Running Shorts', 'Engineered for peak performance, these lightweight running shorts feature built-in compression liners for muscle support and moisture-wicking fabric to keep you dry. The elastic waistband with drawcord provides a secure fit, and the reflective details ensure visibility.', 34.99, '/images/products/running shorts.webp', 150, 0, NULL, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 27, 'Formal Dress Shirt', 'Crafted for a sharp, professional look, this non-iron formal shirt is made from easy-care cotton poplin. It features a classic point collar, single button cuffs, and a tailored fit designed to be worn with a suit or blazer for business or formal occasions.', 64.99, '/images/products/formal shirt.webp', 85, 1, 54.99, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 28, 'Yoga Leggings', 'Find your flow in these high-waisted yoga leggings made from a buttery-soft, four-way stretch fabric. They offer full coverage and support, with a wide waistband that stays in place during any pose. The squat-proof material ensures complete opacity.', 49.99, '/images/products/yoga leggings.webp', 180, 0, NULL, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 29, 'Packable Puffer Vest', 'A versatile layer for unpredictable weather, this lightweight puffer vest is insulated with synthetic down for warmth without bulk. It packs into its own pocket for easy storage, features a stand-up collar, and a water-resistant shell.', 74.99, '/images/products/puffer vest.webp', 95, 1, 59.99, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 30, 'Linen Button-Down Shirt', 'Stay cool and comfortable in this breezy linen shirt, perfect for warm weather and casual outings. Made from 100% natural linen, it features a relaxed fit, button-down collar, and chest pocket. It develops a unique, soft character with every wash.', 69.99, '/images/products/button shirt.webp', 110, 0, NULL, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 31, 'Wireless Bluetooth Earbuds', 'Experience crystal-clear audio and true freedom with these wireless Bluetooth earbuds. Featuring active noise cancellation, they immerse you in your music while blocking out background noise. With a comfortable secure-fit design, IPX5 sweat resistance, and a compact charging case that provides over 24 hours of total battery life.', 129.99, '/images/products/bluetooth earbuds.webp', 150, 1, 99.99, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 32, 'Ultra HD Smart TV', 'Transform your living room with this stunning 55-inch Ultra HD 4K Smart TV. Enjoy breathtaking picture quality with HDR10+ and a wide color gamut. The powerful smart platform gives you instant access to all your favorite streaming apps, and the sleek, bezel-less design looks beautiful from every angle.', 499.99, '/images/products/tv.webp', 25, 0, NULL, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 33, 'Gaming Laptop', 'Dominate the competition with this high-performance gaming laptop. Powered by a latest-gen Intel Core i7 processor and dedicated NVIDIA GeForce RTX graphics, it delivers incredibly smooth frame rates for the most demanding games. The fast 144Hz display and RGB backlit keyboard complete the ultimate gaming setup.', 1499.99, '/images/products/gaming laptop.webp', 30, 1, 1349.99, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 34, 'Wireless Charging Stand', 'Charge your compatible smartphone and earbuds simultaneously with this sleek 3-in-1 wireless charging stand. It features optimized charging coils for fast, efficient power delivery and a stable design that holds your phone at the perfect viewing angle for videos and notifications.', 59.99, '/images/products/charging stand.webp', 200, 0, NULL, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 35, 'Noise Cancelling Headphones', 'Lose yourself in your music with these premium over-ear headphones featuring advanced hybrid active noise cancellation. They offer exceptional sound clarity with deep bass, 30-hour battery life, and plush memory foam ear cups for all-day comfort, making them perfect for travel, work, or relaxation.', 299.99, '/images/products/headphones.webp', 80, 1, 249.99, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 36, 'Smart Home Hub', 'Control your entire smart home with voice commands through this central smart home hub. It connects and unifies thousands of compatible devices—from lights and thermostats to locks and cameras—into a single, easy-to-use app. Enjoy hands-free convenience with built-in voice assistant support.', 129.99, '/images/products/smart hub.webp', 90, 0, NULL, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 37, 'Portable SSD', 'Transfer, back up, and store your files at blazing speeds with this compact portable SSD. With read/write speeds of over 1000MB/s, its perfect for photographers, videographers, and anyone who needs fast, reliable, and durable external storage for large files.', 149.99, '/images/products/portable ssd.webp', 120, 1, 129.99, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 38, 'Mechanical Keyboard', 'Elevate your typing and gaming experience with this responsive mechanical keyboard. Featuring tactile Cherry MX Brown switches, customizable RGB backlighting, and a durable aluminum frame. The N-key rollover and dedicated media controls make it a powerhouse for productivity and play.', 119.99, '/images/products/keyboard.webp', 100, 0, NULL, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 39, 'Action Camera', 'Capture your adventures in stunning 4K video and 20MP photos with this rugged and waterproof action camera. Its equipped with advanced image stabilization for smooth footage, voice control, and a wide range of mounts to capture any perspective, from surfing to skiing.', 249.99, '/images/products/camera.webp', 65, 1, 219.99, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 40, 'E-Reader', 'Carry your entire library in one lightweight, compact device. This e-reader features a high-resolution, glare-free paper-like display that reads like real paper, even in bright sunlight. With weeks of battery life and waterproofing, its the perfect companion for readers everywhere.', 139.99, '/images/products/e-reader.webp', 110, 0, NULL, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 41, 'Hiking Backpack 45L', 'Conquer any trail with this versatile 45-liter hiking backpack. Designed for multi-day trips, it features a lightweight yet durable frame, adjustable torso length, and a ventilated back panel for maximum comfort. Multiple access points, compression straps, and a rain cover make it a reliable companion for all your adventures.', 149.99, '/images/products/backpack.webp', 60, 0, NULL, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 42, 'Trail Running Shoes', 'Tackle rugged terrain with confidence in these lightweight, responsive trail running shoes. They feature aggressive multi-directional lugs for superior grip on muddy and loose trails, a rock plate for underfoot protection, and a secure, comfortable fit that locks your foot in place on technical descents.', 119.99, '/images/products/running shoes.webp', 95, 1, 99.99, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 43, 'Yoga Mat', 'Find your center with our premium extra-thick yoga mat. Made from non-toxic, eco-friendly TPE foam, it provides excellent cushioning for your joints and superior grip to prevent slipping during your practice. The closed-cell surface resists moisture and is easy to clean.', 49.99, '/images/products/yoga mat.webp', 200, 0, NULL, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 44, 'Camping Tent 2-Person', 'Your shelter from the elements on any backpacking trip. This lightweight 2-person tent is incredibly easy to set up with color-coded poles and clips. It features a full-coverage rainfly, mesh walls for ventilation and stargazing, and a compact pack size that wont weigh down your pack.', 199.99, '/images/products/camping tent.webp', 40, 1, 179.99, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 45, 'Stainless Steel Water Bottle', 'Stay hydrated on the go with this durable, insulated stainless steel water bottle. It keeps liquids cold for up to 24 hours or hot for up to 12 hours. The leak-proof cap and wide mouth make it easy to fill, clean, and drink from, while the powder-coated exterior is sweat-free and easy to grip.', 34.99, '/images/products/bottle.webp', 300, 0, NULL, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 46, 'Portable Camping Chair', 'Relax in comfort at the campsite, beach, or backyard with this compact and sturdy portable chair. It features a strong powder-coated steel frame, breathable 600D polyester mesh, and a cup holder in the armrest for your beverage. It folds down small and comes with a carry bag for easy transport.', 44.99, '/images/products/camping chair.webp', 120, 1, 34.99, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 47, 'Adjustable Dumbbell Set', 'Build your home gym without the clutter. This space-saving adjustable dumbbell set allows you to quickly change the weight from 5 to 25 pounds per dumbbell with a simple dial. The compact design and storage tray make it perfect for a full-body workout in a limited space.', 299.99, '/images/products/dumbbell set.webp', 25, 0, NULL, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 48, 'Cycling Helmet', 'Protect your head in style with this lightweight, well-ventilated cycling helmet. Designed with an in-mold polycarbonate shell and EPS foam liner, it meets all safety standards. Features include 18 wind channels for cooling, an adjustable dial fit system, and integrated mounts for a bike light or camera.', 89.99, '/images/products/cycling helmet.webp', 150, 1, 74.99, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 49, 'Compact Binoculars 10x25', 'Bring the distant world closer with these high-performance, compact binoculars. Perfect for birdwatching, hiking, or concerts, they feature 10x magnification, 25mm objective lenses, and multi-coated optics for a bright, clear view. The fold-down design and included case make them highly portable.', 129.99, '/images/products/binoculars.webp', 80, 0, NULL, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 50, 'Foam Roller', 'Speed up recovery and relieve muscle soreness with this high-density foam roller. The textured surface provides a deep tissue massage to target trigger points and improve flexibility. Its an essential tool for post-workout recovery, improving circulation, and reducing muscle tightness.', 29.99, '/images/products/foam roller.webp', 180, 1, 24.99, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 51, 'Building Blocks Set (200pc)', 'Unleash creativity and develop fine motor skills with this classic 200-piece building blocks set. Made from high-quality, non-toxic ABS plastic, the brightly colored blocks are compatible with major brands. Perfect for toddlers and preschoolers to build structures, animals, and anything they can imagine.', 29.99, '/images/products/blocks.webp', 120, 1, 24.99, 6, '2024-09-14 22:40:39.7732556'
UNION SELECT 52, 'Strategy Board Game', 'Gather friends and family for a night of fun with this award-winning strategy board game. Set in a medieval landscape, players compete to build the most prosperous kingdom by claiming land, collecting resources, and deploying clever tactics. With high-quality components and variable setup, no two games are ever the same.', 49.99, '/images/products/board game.webp', 65, 0, NULL, 6, '2024-09-14 22:40:39.7732556'
UNION SELECT 53, 'Remote Control Car', 'Experience high-speed excitement with this 1:16 scale remote control car. Built with a durable ABS body and powerful electric motor, it reaches speeds of up to 15 mph. Features include full-function control (forward, reverse, left, right), working headlights, and a rechargeable battery for hours of fun.', 59.99, '/images/products/rc car.webp', 85, 1, 49.99, 6, '2024-09-14 22:40:39.7732556'
UNION SELECT 54, 'Jigsaw Puzzle (1000pc)', 'Relax and challenge yourself with this beautiful 1000-piece jigsaw puzzle. The high-quality cardboard pieces are precisely cut for a perfect fit and feature a vibrant, matte finish to reduce glare. The finished image is a stunning landscape, perfect for framing after completion.', 19.99, '/images/products/puzzle.webp', 90, 0, NULL, 6, '2024-09-14 22:40:39.7732556'
UNION SELECT 55, 'Plush Stuffed Animal Bear', 'A huggable friend for life! This ultra-soft plush bear is made from high-quality, hypoallergenic materials and features child-safe embroidered eyes. Its super-soft fur and perfect cuddling size make it an ideal companion for naptime, playtime, and comforting little ones.', 24.99, '/images/products/bear.webp', 200, 1, 19.99, 6, '2024-09-14 22:40:39.7732556'
UNION SELECT 56, 'Science Kit for Kids', 'Inspire a love of learning with this exciting STEM science kit. It includes over 20 safe and engaging experiments, from growing crystals to building a simple volcano. The detailed instruction booklet explains the science behind each activity, making learning fun and hands-on.', 34.99, '/images/products/science.webp', 75, 0, NULL, 6, '2024-09-14 22:40:39.7732556'
UNION SELECT 57, 'Electronic Drawing Tablet', 'A mess-free creative studio for budding young artists! This LCD drawing tablet allows kids to create with a stylus or their finger, then erase it all with the press of a button. Its lightweight, portable, and perfect for car rides, restaurants, and home use, helping develop creativity and fine motor skills.', 39.99, '/images/products/tablet.webp', 110, 1, 34.99, 6, '2024-09-14 22:40:39.7732556'
UNION SELECT 58, 'LEGO Creator Set', 'Build, rebuild, and create three different models from one amazing LEGO Creator 3-in-1 set. This medium-complexity set features detailed building techniques and is packed with features and functions, offering hours of imaginative play and display value for builders of all ages.', 69.99, '/images/products/lego.webp', 50, 0, NULL, 6, '2024-09-14 22:40:39.7732556'
UNION SELECT 59, 'Classic Wooden Chess Set', 'Sharpen your strategic mind with this beautifully crafted wooden chess set. The board is made from inlaid walnut and maple wood, and the pieces are hand-carved and weighted for a premium feel. This timeless set is perfect for beginners and seasoned players alike, offering a classic gaming experience.', 79.99, '/images/products/chess.webp', 40, 1, 69.99, 6, '2024-09-14 22:40:39.7732556'
UNION SELECT 60, 'Kids Basketball Hoop', 'Bring the excitement of the court indoors with this adjustable kids basketball hoop. It features a breakaway rim, a soft foam basketball, and an easy-lift system to adjust the height from 4 to 6 feet as your child grows. The stable base can be weighted with sand or water for safety.', 89.99, '/images/products/basketball.webp', 30, 0, NULL, 6, '2024-09-14 22:40:39.7732556'
GO;

INSERT INTO ProductAttribute (Id, [Name], [Value], ProductId)
SELECT 1, 'Power', '120W', 1
UNION SELECT 2, 'Suction Power', '5kPa', 1
UNION SELECT 3, 'Cord Length', '4.5m', 1
UNION SELECT 4, 'Weight', '2.5kg', 1
UNION SELECT 5, 'Filter Type', 'Washable', 1
UNION SELECT 6, 'Dimensions', '30x15x10cm', 1
UNION SELECT 7, 'Noise Level', '75dB', 1
UNION SELECT 8, 'Capacity', '0.5L', 1
UNION SELECT 9, 'Voltage', '12V', 1
UNION SELECT 10, 'Color', 'Black', 1
UNION SELECT 11, 'Resolution', '1080p Full HD', 2
UNION SELECT 12, 'Viewing Angle', '170 degrees', 2
UNION SELECT 13, 'Night Vision', 'Yes', 2
UNION SELECT 14, 'GPS', 'Yes', 2
UNION SELECT 15, 'G-Sensor', 'Yes', 2
UNION SELECT 16, 'Loop Recording', 'Yes', 2
UNION SELECT 17, 'Screen Size', '2.5 inches', 2
UNION SELECT 18, 'Storage', 'MicroSD up to 128GB', 2
UNION SELECT 19, 'Mounting', 'Suction Cup', 2
UNION SELECT 20, 'Power Source', '12V Car Adapter', 2
UNION SELECT 21, 'Material', 'Polyester Blend', 3
UNION SELECT 22, 'Waterproof', 'Yes', 3
UNION SELECT 23, 'UV Protection', 'Yes', 3
UNION SELECT 24, 'Dimensions', '500x200x150cm', 3
UNION SELECT 25, 'Weight', '2.8kg', 3
UNION SELECT 26, 'Fit Type', 'Universal', 3
UNION SELECT 27, 'Elastic Hems', 'Yes', 3
UNION SELECT 28, 'Storage Bag', 'Included', 3
UNION SELECT 29, 'Color', 'Gray', 3
UNION SELECT 30, 'Season', 'All-Weather', 3
UNION SELECT 31, 'Power Source', '12V DC', 4
UNION SELECT 32, 'Max Pressure', '150 PSI', 4
UNION SELECT 33, 'Digital Display', 'Yes', 4
UNION SELECT 34, 'Auto Shut-off', 'Yes', 4
UNION SELECT 35, 'LED Light', 'Yes', 4
UNION SELECT 36, 'Nozzle Attachments', '3', 4
UNION SELECT 37, 'Weight', '1.2kg', 4
UNION SELECT 38, 'Dimensions', '25x15x10cm', 4
UNION SELECT 39, 'Cord Length', '3m', 4
UNION SELECT 40, 'Inflation Time', '5 minutes', 4
UNION SELECT 41, 'Material', '600D Polyester', 5
UNION SELECT 42, 'Pockets', '8', 5
UNION SELECT 43, 'Drink Holders', '2', 5
UNION SELECT 44, 'Tablet Pocket', 'Yes', 5
UNION SELECT 45, 'Attachment', 'Adjustable Straps', 5
UNION SELECT 46, 'Dimensions', '40x35x15cm', 5
UNION SELECT 47, 'Weight', '0.5kg', 5
UNION SELECT 48, 'Color', 'Black', 5
UNION SELECT 49, 'Water Resistant', 'Yes', 5
UNION SELECT 50, 'Mounting', 'Seat Back', 5
UNION SELECT 51, 'Material', 'Genuine Leather', 6
UNION SELECT 52, 'Grip', 'Non-slip', 6
UNION SELECT 53, 'Installation', 'No Tools Required', 6
UNION SELECT 54, 'Dimensions', '38-40cm diameter', 6
UNION SELECT 55, 'Thickness', '3mm', 6
UNION SELECT 56, 'Color', 'Black', 6
UNION SELECT 57, 'Stitching', 'Contrast Stitch', 6
UNION SELECT 58, 'Compatibility', 'Universal', 6
UNION SELECT 59, 'Heat Resistance', 'Yes', 6
UNION SELECT 60, 'Cold Resistance', 'Yes', 6
UNION SELECT 61, 'Fragrance', 'New Car Scent', 7
UNION SELECT 62, 'Duration', '60 days', 7
UNION SELECT 63, 'Type', 'Vent Clip', 7
UNION SELECT 64, 'Material', 'ABS Plastic', 7
UNION SELECT 65, 'Dimensions', '8x5x3cm', 7
UNION SELECT 66, 'Weight', '50g', 7
UNION SELECT 67, 'Adjustable', 'Yes', 7
UNION SELECT 68, 'Natural Ingredients', 'Yes', 7
UNION SELECT 69, 'Pack Quantity', '1', 7
UNION SELECT 70, 'Color', 'Black', 7
UNION SELECT 71, 'Capacity', '10000mAh', 8
UNION SELECT 72, 'Peak Current', '600A', 8
UNION SELECT 73, 'USB Ports', '2', 8
UNION SELECT 74, 'LED Flashlight', 'Yes', 8
UNION SELECT 75, 'Safety Protection', 'Yes', 8
UNION SELECT 76, 'Dimensions', '15x8x3cm', 8
UNION SELECT 77, 'Weight', '0.6kg', 8
UNION SELECT 78, 'Charging Time', '4 hours', 8
UNION SELECT 79, 'Compatibility', 'Cars,Trucks,SUVs', 8
UNION SELECT 80, 'Warranty', '1 year', 8
UNION SELECT 81, 'Material', 'Reflective Polyester', 9
UNION SELECT 82, 'UV Protection', '99%', 9
UNION SELECT 83, 'Foldable', 'Yes', 9
UNION SELECT 84, 'Dimensions', '140x70cm', 9
UNION SELECT 85, 'Weight', '0.3kg', 9
UNION SELECT 86, 'Storage Case', 'Included', 9
UNION SELECT 87, 'Color', 'Silver', 9
UNION SELECT 88, 'Compatibility', 'Universal', 9
UNION SELECT 89, 'Heat Reduction', 'Up to 30°C', 9
UNION SELECT 90, 'Easy Installation', 'Yes', 9
UNION SELECT 91, 'Brightness', '6000 Lumens', 10
UNION SELECT 92, 'Color Temperature', '6000K', 10
UNION SELECT 93, 'Beam Pattern', 'Projector', 10
UNION SELECT 94, 'Lifespan', '50000 hours', 10
UNION SELECT 95, 'Power Consumption', '30W', 10
UNION SELECT 96, 'Waterproof Rating', 'IP67', 10
UNION SELECT 97, 'Installation', 'Plug and Play', 10
UNION SELECT 98, 'Compatibility', 'Universal', 10
UNION SELECT 99, 'Voltage', '12V', 10
UNION SELECT 100, 'Warranty', '2 years', 10
UNION SELECT 101, 'Author', 'F. Scott Fitzgerald', 11
UNION SELECT 102, 'Pages', '180', 11
UNION SELECT 103, 'Publisher', 'Scribner', 11
UNION SELECT 104, 'Language', 'English', 11
UNION SELECT 105, 'ISBN', '9780743273565', 11
UNION SELECT 106, 'Genre', 'Classic Fiction', 11
UNION SELECT 107, 'Format', 'Paperback', 11
UNION SELECT 108, 'Dimensions', '20x13x2cm', 11
UNION SELECT 109, 'Weight', '200g', 11
UNION SELECT 110, 'Publication Year', '1925', 11
UNION SELECT 111, 'Author', 'Harper Lee', 12
UNION SELECT 112, 'Pages', '281', 12
UNION SELECT 113, 'Publisher', 'J.B. Lippincott', 12
UNION SELECT 114, 'Language', 'English', 12
UNION SELECT 115, 'ISBN', '9780061120084', 12
UNION SELECT 116, 'Genre', 'Classic Fiction', 12
UNION SELECT 117, 'Format', 'Hardcover', 12
UNION SELECT 118, 'Dimensions', '21x14x3cm', 12
UNION SELECT 119, 'Weight', '350g', 12
UNION SELECT 120, 'Publication Year', '1960', 12
UNION SELECT 121, 'Author', 'George Orwell', 13
UNION SELECT 122, 'Pages', '328', 13
UNION SELECT 123, 'Publisher', 'Secker and Warburg', 13
UNION SELECT 124, 'Language', 'English', 13
UNION SELECT 125, 'ISBN', '9780451524935', 13
UNION SELECT 126, 'Genre', 'Dystopian Fiction', 13
UNION SELECT 127, 'Format', 'Paperback', 13
UNION SELECT 128, 'Dimensions', '19x12x2cm', 13
UNION SELECT 129, 'Weight', '250g', 13
UNION SELECT 130, 'Publication Year', '1949', 13
UNION SELECT 131, 'Author', 'Jane Austen', 14
UNION SELECT 132, 'Pages', '432', 14
UNION SELECT 133, 'Publisher', 'T. Egerton', 14
UNION SELECT 134, 'Language', 'English', 14
UNION SELECT 135, 'ISBN', '9780141439518', 14
UNION SELECT 136, 'Genre', 'Romance', 14
UNION SELECT 137, 'Format', 'Paperback', 14
UNION SELECT 138, 'Dimensions', '19x12x3cm', 14
UNION SELECT 139, 'Weight', '280g', 14
UNION SELECT 140, 'Publication Year', '1813', 14
UNION SELECT 141, 'Author', 'J.R.R. Tolkien', 15
UNION SELECT 142, 'Pages', '310', 15
UNION SELECT 143, 'Publisher', 'Allen & Unwin', 15
UNION SELECT 144, 'Language', 'English', 15
UNION SELECT 145, 'ISBN', '9780547928227', 15
UNION SELECT 146, 'Genre', 'Fantasy', 15
UNION SELECT 147, 'Format', 'Hardcover', 15
UNION SELECT 148, 'Dimensions', '22x14x3cm', 15
UNION SELECT 149, 'Weight', '400g', 15
UNION SELECT 150, 'Publication Year', '1937', 15
UNION SELECT 151, 'Author', 'J.K. Rowling', 16
UNION SELECT 152, 'Pages', '320', 16
UNION SELECT 153, 'Publisher', 'Bloomsbury', 16
UNION SELECT 154, 'Language', 'English', 16
UNION SELECT 155, 'ISBN', '9780747532743', 16
UNION SELECT 156, 'Genre', 'Fantasy', 16
UNION SELECT 157, 'Format', 'Paperback', 16
UNION SELECT 158, 'Dimensions', '19x12x2cm', 16
UNION SELECT 159, 'Weight', '220g', 16
UNION SELECT 160, 'Publication Year', '1997', 16
UNION SELECT 161, 'Author', 'J.D. Salinger', 17
UNION SELECT 162, 'Pages', '234', 17
UNION SELECT 163, 'Publisher', 'Little, Brown and Company', 17
UNION SELECT 164, 'Language', 'English', 17
UNION SELECT 165, 'ISBN', '9780316769174', 17
UNION SELECT 166, 'Genre', 'Literary Fiction', 17
UNION SELECT 167, 'Format', 'Paperback', 17
UNION SELECT 168, 'Dimensions', '17x10x2cm', 17
UNION SELECT 169, 'Weight', '180g', 17
UNION SELECT 170, 'Publication Year', '1951', 17
UNION SELECT 171, 'Author', 'William Golding', 18
UNION SELECT 172, 'Pages', '224', 18
UNION SELECT 173, 'Publisher', 'Faber and Faber', 18
UNION SELECT 174, 'Language', 'English', 18
UNION SELECT 175, 'ISBN', '9780571056866', 18
UNION SELECT 176, 'Genre', 'Allegory', 18
UNION SELECT 177, 'Format', 'Paperback', 18
UNION SELECT 178, 'Dimensions', '19x12x2cm', 18
UNION SELECT 179, 'Weight', '190g', 18
UNION SELECT 180, 'Publication Year', '1954', 18
UNION SELECT 181, 'Author', 'Paulo Coelho', 19
UNION SELECT 182, 'Pages', '208', 19
UNION SELECT 183, 'Publisher', 'HarperTorch', 19
UNION SELECT 184, 'Language', 'English', 19
UNION SELECT 185, 'ISBN', '9780061122415', 19
UNION SELECT 186, 'Genre', 'Allegorical Fiction', 19
UNION SELECT 187, 'Format', 'Paperback', 19
UNION SELECT 188, 'Dimensions', '17x10x2cm', 19
UNION SELECT 189, 'Weight', '170g', 19
UNION SELECT 190, 'Publication Year', '1988', 19
UNION SELECT 191, 'Author', 'Dan Brown', 20
UNION SELECT 192, 'Pages', '489', 20
UNION SELECT 193, 'Publisher', 'Doubleday', 20
UNION SELECT 194, 'Language', 'English', 20
UNION SELECT 195, 'ISBN', '9780307474278', 20
UNION SELECT 196, 'Genre', 'Thriller', 20
UNION SELECT 197, 'Format', 'Hardcover', 20
UNION SELECT 198, 'Dimensions', '24x16x4cm', 20
UNION SELECT 199, 'Weight', '550g', 20
UNION SELECT 200, 'Publication Year', '2003', 20
UNION SELECT 201, 'Material', '100% Combed Cotton', 21
UNION SELECT 202, 'Fit', 'Classic', 21
UNION SELECT 203, 'Neckline', 'Crewneck', 21
UNION SELECT 204, 'Sleeve Length', 'Short', 21
UNION SELECT 205, 'Sizes', 'XS, S, M, L, XL', 21
UNION SELECT 206, 'Colors', 'Black, White, Gray, Navy', 21
UNION SELECT 207, 'Care', 'Machine Wash Cold', 21
UNION SELECT 208, 'Weight', '180 gsm', 21
UNION SELECT 209, 'Origin', 'Imported', 21
UNION SELECT 210, 'Activity', 'Hiking & Outdoor', 21
UNION SELECT 211, 'Material', '98% Cotton 2% Elastane', 22
UNION SELECT 212, 'Fit', 'Slim', 22
UNION SELECT 213, 'Rise', 'Mid Rise', 22
UNION SELECT 214, 'Inseam', '32 inches', 22
UNION SELECT 215, 'Sizes', '28, 30, 32, 34, 36', 22
UNION SELECT 216, 'Colors', 'Dark Blue, Light Wash, Black', 22
UNION SELECT 217, 'Closure', 'Button Fly & Zip', 22
UNION SELECT 218, 'Pockets', '5 Pockets', 22
UNION SELECT 219, 'Leg Opening', '14 inches', 22
UNION SELECT 220, 'Activity', 'Hiking & Outdoor', 22
UNION SELECT 221, 'Material', 'Brushed Cotton Fleece', 23
UNION SELECT 222, 'Fit', 'Regular', 23
UNION SELECT 223, 'Sleeve Length', 'Long', 23
UNION SELECT 224, 'Neckline', 'Hooded', 23
UNION SELECT 225, 'Pockets', 'Kangaroo Pocket', 23
UNION SELECT 226, 'Sizes', 'S, M, L, XL, XXL', 23
UNION SELECT 227, 'Colors', 'Heather Gray, Navy, Burgundy', 23
UNION SELECT 228, 'Care', 'Machine Washable', 23
UNION SELECT 229, 'Weight', '400 gsm', 23
UNION SELECT 230, 'Activity', 'Hiking & Outdoor', 23
UNION SELECT 231, 'Material', 'Polyester with Membrane', 24
UNION SELECT 232, 'Waterproof Rating', '10000mm', 24
UNION SELECT 233, 'Windproof', 'Yes', 24
UNION SELECT 234, 'Breathability', '5000g/m²/24hr', 24
UNION SELECT 235, 'Hood', 'Adjustable', 24
UNION SELECT 236, 'Pockets', '2 Zippered Hand', 24
UNION SELECT 237, 'Sizes', 'M, L, XL, XXL', 24
UNION SELECT 238, 'Colors', 'Black, Navy, Olive Green', 24
UNION SELECT 239, 'Packability', 'Packable', 24
UNION SELECT 240, 'Activity', 'Hiking & Outdoor', 24
UNION SELECT 241, 'Material', '70% Wool 30% Acrylic', 25
UNION SELECT 242, 'Fit', 'Relaxed', 25
UNION SELECT 243, 'Neckline', 'Crewneck', 25
UNION SELECT 244, 'Pattern', 'Cable Knit', 25
UNION SELECT 245, 'Sleeve Length', 'Long', 25
UNION SELECT 246, 'Sizes', 'S, M, L, XL', 25
UNION SELECT 247, 'Colors', 'Cream, Charcoal, Burgundy', 25
UNION SELECT 248, 'Care', 'Hand Wash Cold', 25
UNION SELECT 249, 'Style', 'Casual', 25
UNION SELECT 250, 'Activity', 'Hiking & Outdoor', 25
UNION SELECT 251, 'Material', 'Polyester & Spandex', 26
UNION SELECT 252, 'Fit', 'Active', 26
UNION SELECT 253, 'Length', '5-inch Inseam', 26
UNION SELECT 254, 'Liner', 'Compression Liner', 26
UNION SELECT 255, 'Moisture Wicking', 'Yes', 26
UNION SELECT 256, 'Pockets', '1 Zippered Back', 26
UNION SELECT 257, 'Sizes', 'S, M, L, XL', 26
UNION SELECT 258, 'Colors', 'Black, Royal Blue, Electric Green', 26
UNION SELECT 259, 'Reflective Details', 'Yes', 26
UNION SELECT 260, 'Activity', 'Running', 26
UNION SELECT 261, 'Material', '100% Cotton Poplin', 27
UNION SELECT 262, 'Fit', 'Tailored', 27
UNION SELECT 263, 'Collar', 'Point Collar', 27
UNION SELECT 264, 'Cuff', 'Single Button', 27
UNION SELECT 265, 'Non-Iron', 'Yes', 27
UNION SELECT 266, 'Sizes', '15, 15.5, 16, 16.5, 17', 27
UNION SELECT 267, 'Colors', 'White, Light Blue, Pale Pink', 27
UNION SELECT 268, 'Care', 'Machine Wash', 27
UNION SELECT 269, 'Occasion', 'Formal & Business', 27
UNION SELECT 270, 'Activity', 'Hiking & Outdoor', 27
UNION SELECT 271, 'Material', 'Nylon & Spandex', 28
UNION SELECT 272, 'Fit', 'High-Waisted', 28
UNION SELECT 273, 'Length', 'Full Length', 28
UNION SELECT 274, 'Opacity', 'Squat-Proof', 28
UNION SELECT 275, 'Stretch', '4-Way Stretch', 28
UNION SELECT 276, 'Sizes', 'XS, S, M, L, XL', 28
UNION SELECT 277, 'Colors', 'Black, Marble, Deep Teal', 28
UNION SELECT 278, 'Pockets', 'None', 28
UNION SELECT 279, 'Activity', 'Yoga & Fitness', 28
UNION SELECT 280, 'Leg Opening', '13 inches', 28
UNION SELECT 281, 'Material', 'Polyester Taffeta', 29
UNION SELECT 282, 'Insulation', 'Synthetic Down', 29
UNION SELECT 283, 'Weight', 'Lightweight', 29
UNION SELECT 284, 'Packable', 'Packs into own pocket', 29
UNION SELECT 285, 'Collar', 'Stand-up Collar', 29
UNION SELECT 286, 'Sizes', 'S, M, L, XL', 29
UNION SELECT 287, 'Colors', 'Black, Olive, Bright Red', 29
UNION SELECT 288, 'Water Resistance', 'Water-Resistant', 29
UNION SELECT 289, 'Fill Power', '60g', 29
UNION SELECT 290, 'Opening', 'Medium', 29
UNION SELECT 291, 'Material', '100% Linen', 30
UNION SELECT 292, 'Fit', 'Relaxed', 30
UNION SELECT 293, 'Collar', 'Button-Down', 30
UNION SELECT 294, 'Sleeve Length', 'Long', 30
UNION SELECT 295, 'Pockets', '1 Chest Pocket', 30
UNION SELECT 296, 'Sizes', 'S, M, L, XL', 30
UNION SELECT 297, 'Colors', 'Natural, Light Blue, Khaki', 30
UNION SELECT 298, 'Breathability', 'Highly Breathable', 30
UNION SELECT 299, 'Wrinkle', 'Naturally Wrinkled', 30
UNION SELECT 300, 'Opening', 'Large', 30
GO;

INSERT INTO ProductAttribute (Id, [Name], [Value], ProductId)
SELECT 301, 'Connectivity', 'Bluetooth 5.2', 31
UNION SELECT 302, 'Battery Life', '8 hours (24h with case)', 31
UNION SELECT 303, 'Charging', 'USB-C Wireless', 31
UNION SELECT 304, 'Noise Cancellation', 'Active', 31
UNION SELECT 305, 'Water Resistance', 'IPX5', 31
UNION SELECT 306, 'Driver Size', '10mm', 31
UNION SELECT 307, 'Weight', '5g per earbud', 31
UNION SELECT 308, 'Charging Time', '2 hours', 31
UNION SELECT 309, 'Compatibility', 'Universal', 31
UNION SELECT 310, 'Screen Size', '55 inches', 32
UNION SELECT 311, 'Resolution', '3840x2160 (4K UHD)', 32
UNION SELECT 312, 'Display Technology', 'QLED', 32
UNION SELECT 313, 'Smart Platform', 'WebOS', 32
UNION SELECT 314, 'HDR', 'HDR10+', 32
UNION SELECT 315, 'Refresh Rate', '60Hz', 32
UNION SELECT 316, 'Ports', '4x HDMI, 3x USB', 32
UNION SELECT 317, 'Voice Control', 'Google Assistant', 32
UNION SELECT 318, 'Dimensions', '123.2x71.7x8.5cm', 32
UNION SELECT 319, 'Processor', 'Intel Core i7-12700H', 33
UNION SELECT 320, 'Graphics', 'NVIDIA GeForce RTX 4060', 33
UNION SELECT 321, 'RAM', '16GB DDR5', 33
UNION SELECT 322, 'Storage', '1TB NVMe SSD', 33
UNION SELECT 323, 'Display', '15.6 144Hz', 33
UNION SELECT 324, 'Operating System', 'Windows 11', 33
UNION SELECT 325, 'Keyboard', 'RGB Backlit', 33
UNION SELECT 326, 'Weight', '2.3kg', 33
UNION SELECT 327, 'Battery Life', '6 hours', 33
UNION SELECT 328, 'Charging Standard', 'Qi', 34
UNION SELECT 329, 'Output', '15W Max', 34
UNION SELECT 330, 'Compatibility', 'iPhone, Samsung, AirPods', 34
UNION SELECT 331, 'Input', 'USB-C', 34
UNION SELECT 332, 'Charging Points', '3', 34
UNION SELECT 333, 'LED Indicator', 'Yes', 34
UNION SELECT 334, 'Cable Length', '1.5m', 34
UNION SELECT 335, 'Material', 'ABS Plastic', 34
UNION SELECT 336, 'Weight', '250g', 34
UNION SELECT 337, 'Noise Cancellation', 'Hybrid Active', 35
UNION SELECT 338, 'Battery Life', '30 hours', 35
UNION SELECT 339, 'Connectivity', 'Bluetooth 5.0 & 3.5mm', 35
UNION SELECT 340, 'Driver Size', '40mm', 35
UNION SELECT 341, 'Weight', '255g', 35
UNION SELECT 342, 'Ear Cup Material', 'Memory Foam', 35
UNION SELECT 343, 'Charging', 'USB-C', 35
UNION SELECT 344, 'Foldable', 'Yes', 35
UNION SELECT 345, 'Carrying Case', 'Included', 35
UNION SELECT 346, 'Connectivity', 'Wi-Fi Ethernet Zigbee', 36
UNION SELECT 347, 'Voice Assistant', 'Built-in', 36
UNION SELECT 348, 'Compatibility', 'Works with Alexa, Google Home', 36
UNION SELECT 349, 'Mobile App', 'Yes', 36
UNION SELECT 350, 'Speaker', 'Yes', 36
UNION SELECT 351, 'Microphone Array', '4 far-field mics', 36
UNION SELECT 352, 'Dimensions', '10x10x5cm', 36
UNION SELECT 353, 'Color', 'Black', 36
UNION SELECT 354, 'Power Supply', 'AC Adapter', 36
UNION SELECT 355, 'Capacity', '1TB', 37
UNION SELECT 356, 'Interface', 'USB 3.2 Gen 2', 37
UNION SELECT 357, 'Read Speed', '1050 MB/s', 37
UNION SELECT 358, 'Write Speed', '1000 MB/s', 37
UNION SELECT 359, 'Encryption', '256-bit AES', 37
UNION SELECT 360, 'Dimensions', '8.5x5x1cm', 37
UNION SELECT 361, 'Weight', '45g', 37
UNION SELECT 362, 'Durability', 'Shock Resistant', 37
UNION SELECT 363, 'Compatibility', 'PC, Mac, PS5, Xbox', 37
UNION SELECT 364, 'Included Cable', 'USB-C to C/A', 37
UNION SELECT 365, 'Switch Type', 'Cherry MX Brown', 38
UNION SELECT 366, 'Backlighting', 'RGB', 38
UNION SELECT 367, 'Key Rollover', 'N-Key', 38
UNION SELECT 368, 'Layout', 'Full Size (104-key)', 38
UNION SELECT 369, 'Material', 'Aluminum Frame', 38
UNION SELECT 370, 'Connectivity', 'USB', 38
UNION SELECT 371, 'Media Controls', 'Dedicated', 38
UNION SELECT 372, 'Weight', '1.1kg', 38
UNION SELECT 373, 'Software', 'Macro Programming', 38
UNION SELECT 374, 'Video Resolution', '4K at 60fps', 39
UNION SELECT 375, 'Photo Resolution', '20MP', 39
UNION SELECT 376, 'Stabilization', 'Advanced Electronic', 39
UNION SELECT 377, 'Waterproof', '10m (40m with case)', 39
UNION SELECT 378, 'Connectivity', 'Wi-Fi & Bluetooth', 39
UNION SELECT 379, 'Screen', '2-inch Touch', 39
UNION SELECT 380, 'Voice Control', 'Yes', 39
UNION SELECT 381, 'Battery Life', '90 minutes', 39
UNION SELECT 382, 'Storage', 'microSD', 39
UNION SELECT 383, 'Screen Size', '6.8 inches', 40
UNION SELECT 384, 'Screen Type', 'E Ink Carta', 40
UNION SELECT 385, 'Resolution', '300 PPI', 40
UNION SELECT 386, 'Waterproof', 'IPX8', 40
UNION SELECT 387, 'Battery Life', 'Weeks', 40
UNION SELECT 388, 'Storage', '8GB', 40
UNION SELECT 389, 'Connectivity', 'Wi-Fi', 40
UNION SELECT 390, 'Weight', '188g', 40
UNION SELECT 391, 'Format Support', 'EPUB, PDF, MOBI', 40
UNION SELECT 392, 'Capacity', '45 Liters', 41
UNION SELECT 393, 'Material', 'Ripstop Nylon', 41
UNION SELECT 394, 'Frame Type', 'Internal Frame', 41
UNION SELECT 395, 'Suspension', 'Adjustable', 41
UNION SELECT 396, 'Torso Adjustment', 'Yes', 41
UNION SELECT 397, 'Hydration Compatible', 'Yes', 41
UNION SELECT 398, 'Rain Cover', 'Included', 41
UNION SELECT 399, 'Weight', '1.4kg', 41
UNION SELECT 400, 'Gender', 'Unisex', 41
UNION SELECT 401, 'Type', 'Trail Running', 42
UNION SELECT 402, 'Drop', '8mm', 42
UNION SELECT 403, 'Cushioning', 'Responsive', 42
UNION SELECT 404, 'Outsole', 'Vibram Megagrip', 42
UNION SELECT 405, 'Toe Protection', 'Reinforced', 42
UNION SELECT 406, 'Water Resistance', 'No', 42
UNION SELECT 407, 'Weight', '290g (per shoe)', 42
UNION SELECT 408, 'Closure', 'Laces', 42
UNION SELECT 409, 'Sizes', 'US 7-13', 42
UNION SELECT 410, 'Width', 'Standard', 42
UNION SELECT 411, 'Material', 'TPE Foam', 43
UNION SELECT 412, 'Thickness', '6mm', 43
UNION SELECT 413, 'Length', '183cm', 43
UNION SELECT 414, 'Width', '61cm', 43
UNION SELECT 415, 'Weight', '1.2kg', 43
UNION SELECT 416, 'Surface', 'Non-slip', 43
UNION SELECT 417, 'Grip', 'Superior', 43
UNION SELECT 418, 'Color', 'Purple, Blue, Green', 43
UNION SELECT 419, 'Eco-Friendly', 'Yes', 43
UNION SELECT 420, 'Carry Strap', 'Included', 43
UNION SELECT 421, 'Capacity', '2 Person', 44
UNION SELECT 422, 'Seasons', '3-Season', 44
UNION SELECT 423, 'Doors', '1', 44
UNION SELECT 424, 'Windows', 'Mesh Panels', 44
UNION SELECT 425, 'Weight', '2.1kg', 44
UNION SELECT 426, 'Packed Size', '45x15cm', 44
UNION SELECT 427, 'Poles', 'Aluminum', 44
UNION SELECT 428, 'Waterproof Rating', '3000mm', 44
UNION SELECT 429, 'Fly Material', 'Polyester', 44
UNION SELECT 430, 'Floor Material', 'Polyethylene', 44
UNION SELECT 431, 'Capacity', '1 Liter', 45
UNION SELECT 432, 'Material', '18/8 Stainless Steel', 45
UNION SELECT 433, 'Insulation', 'Double-Wall Vacuum', 45
UNION SELECT 434, 'Leak Proof', 'Yes', 45
UNION SELECT 435, 'Lid Type', 'Screw-top with Loop', 45
UNION SELECT 436, 'Cold Retention', '24 Hours', 45
UNION SELECT 437, 'Hot Retention', '12 Hours', 45
UNION SELECT 438, 'Weight', '380g', 45
UNION SELECT 439, 'Dishwasher Safe', 'Yes', 45
UNION SELECT 440, 'Weight Capacity', '120kg', 46
UNION SELECT 441, 'Frame Material', 'Powder-Coated Steel', 46
UNION SELECT 442, 'Seat Material', '600D Polyester', 46
UNION SELECT 443, 'Weight', '2.5kg', 46
UNION SELECT 444, 'Folded Size', '90x15x15cm', 46
UNION SELECT 445, 'Features', 'Cup Holder', 46
UNION SELECT 446, 'Carry Bag', 'Included', 46
UNION SELECT 447, 'Leg Tips', 'Yes', 46
UNION SELECT 448, 'Color', 'Blue, Gray, Black', 46
UNION SELECT 449, 'Weight Range', '5-25 lbs per dumbbell', 47
UNION SELECT 450, 'Increment', '2.5 lbs', 47
UNION SELECT 451, 'Adjustment Mechanism', 'Dial System', 47
UNION SELECT 452, 'Storage', 'Tray Included', 47
UNION SELECT 453, 'Grip', 'Textured', 47
UNION SELECT 454, 'Material', 'Chromed Steel', 47
UNION SELECT 455, 'Total Weight', '50 lbs', 47
UNION SELECT 456, 'Dimensions', '45x25x20cm', 47
UNION SELECT 457, 'Workout', 'Full Body', 47
UNION SELECT 458, 'Safety Standard', 'CE EN1078', 48
UNION SELECT 459, 'Vents', '18', 48
UNION SELECT 460, 'Fit System', 'Adjustable Dial', 48
UNION SELECT 461, 'Weight', '280g', 48
UNION SELECT 462, 'Shell Type', 'In-Mold', 48
UNION SELECT 463, 'Visor', 'Removable', 48
UNION SELECT 464, 'Mounts', 'Light/Camera', 48
UNION SELECT 465, 'Size Adjustment', 'Yes', 48
UNION SELECT 466, 'Sizes', 'S, M, L', 48
UNION SELECT 467, 'Magnification', '10x', 49
UNION SELECT 468, 'Objective Lens Diameter', '25mm', 49
UNION SELECT 469, 'Prism Type', 'BaK-4 Roof', 49
UNION SELECT 470, 'Focus System', 'Center', 49
UNION SELECT 471, 'Close Focus', '2m', 49
UNION SELECT 472, 'Water Resistance', 'Yes', 49
UNION SELECT 473, 'Weight', '450g', 49
UNION SELECT 474, 'Eye Relief', '15mm', 49
UNION SELECT 475, 'Included', 'Case & Strap', 49
UNION SELECT 476, 'Material', 'EVA Foam', 50
UNION SELECT 477, 'Length', '33cm', 50
UNION SELECT 478, 'Diameter', '14cm', 50
UNION SELECT 479, 'Surface', 'Textured', 50
UNION SELECT 480, 'Color', 'Blue', 50
UNION SELECT 481, 'Weight', '0.5kg', 50
UNION SELECT 482, 'Intensity', 'Medium-Firm', 50
UNION SELECT 483, 'Use', 'Muscle Recovery', 50
UNION SELECT 484, 'Portability', 'Lightweight', 50
UNION SELECT 485, 'Number of Pieces', '200', 51
UNION SELECT 486, 'Material', 'Non-toxic ABS Plastic', 51
UNION SELECT 487, 'Age Range', '2-6 years', 51
UNION SELECT 488, 'Block Size', 'Standard', 51
UNION SELECT 489, 'Compatibility', 'Major Brands', 51
UNION SELECT 490, 'Skills', 'Creativity, Motor Skills', 51
UNION SELECT 491, 'Storage', 'Storage Bucket', 51
UNION SELECT 492, 'Weight', '1.8kg', 51
UNION SELECT 493, 'Colors', 'Multicolor', 51
UNION SELECT 494, 'Players', '2-4', 52
UNION SELECT 495, 'Play Time', '60-90 minutes', 52
UNION SELECT 496, 'Age Range', '10+', 52
UNION SELECT 497, 'Game Type', 'Strategy Eurogame', 52
UNION SELECT 498, 'Contents', 'Board, Cards, Wooden Pieces', 52
UNION SELECT 499, 'Skills', 'Strategy, Resource Management', 52
UNION SELECT 500, 'Awards', 'Spiel des Jahres', 52
UNION SELECT 501, 'Dimensions', '30x30x7cm', 52
UNION SELECT 502, 'Weight', '1.5kg', 52
UNION SELECT 503, 'Scale', '1:16', 53
UNION SELECT 504, 'Power Source', 'Rechargeable Battery', 53
UNION SELECT 505, 'Control Range', '50 meters', 53
UNION SELECT 506, 'Max Speed', '15 mph', 53
UNION SELECT 507, 'Charging Time', '2 hours', 53
UNION SELECT 508, 'Play Time', '30 minutes', 53
UNION SELECT 509, 'Material', 'ABS Plastic', 53
UNION SELECT 510, 'Features', 'Working Headlights', 53
UNION SELECT 511, 'Suitable For', 'Indoor & Outdoor', 53
UNION SELECT 512, 'Age Range', '8+ years', 53
UNION SELECT 513, 'Piece Count', '1000', 54
UNION SELECT 514, 'Piece Cut', 'Precision Cut', 54
UNION SELECT 515, 'Finish', 'Matte Finish', 54
UNION SELECT 516, 'Material', 'Cardboard', 54
UNION SELECT 517, 'Finished Size', '68x48cm', 54
UNION SELECT 518, 'Age Range', '12+', 54
UNION SELECT 519, 'Difficulty', 'Intermediate', 54
UNION SELECT 520, 'Theme', 'Landscape', 54
UNION SELECT 521, 'Box Dimensions', '30x20x5cm', 54
UNION SELECT 522, 'Material', 'Polyester Plush', 55
UNION SELECT 523, 'Height', '35cm', 55
UNION SELECT 524, 'Stuffing', 'Hypoallergenic Fiber', 55
UNION SELECT 525, 'Features', 'Embroidered Eyes', 55
UNION SELECT 526, 'Age Range', '0+', 55
UNION SELECT 527, 'Machine Washable', 'Yes', 55
UNION SELECT 528, 'Color', 'Brown', 55
UNION SELECT 529, 'Weight', '0.4kg', 55
UNION SELECT 530, 'Safety', 'Child Safe', 55
UNION SELECT 531, 'Experiments', '20+', 56
UNION SELECT 532, 'Theme', 'General Science', 56
UNION SELECT 533, 'Age Range', '6-12 years', 56
UNION SELECT 534, 'STEM Focus', 'Chemistry Geology', 56
UNION SELECT 535, 'Contents', 'Lab Tools, Chemicals, Guide', 56
UNION SELECT 536, 'Adult Supervision', 'Recommended', 56
UNION SELECT 537, 'Skills', 'Critical Thinking, Curiosity', 56
UNION SELECT 538, 'Weight', '0.9kg', 56
UNION SELECT 539, 'Dimensions', '25x20x8cm', 56
UNION SELECT 540, 'Screen Size', '8.5 inches', 57
UNION SELECT 541, 'Screen Type', 'Pressure-sensitive LCD', 57
UNION SELECT 542, 'Power', 'CR2032 Battery', 57
UNION SELECT 543, 'Erase Method', 'One-touch Button', 57
UNION SELECT 544, 'Weight', '0.2kg', 57
UNION SELECT 545, 'Age Range', '3+ years', 57
UNION SELECT 546, 'Portability', 'High', 57
UNION SELECT 547, 'Included', 'Stylus', 57
UNION SELECT 548, 'Dimensions', '22x15x1cm', 57
UNION SELECT 549, 'Skills', 'Creativity', 57
UNION SELECT 550, 'Piece Count', '654', 58
UNION SELECT 551, 'Models', '3-in-1', 58
UNION SELECT 552, 'Age Range', '9-14 years', 58
UNION SELECT 553, 'Theme', 'Creator Expert', 58
UNION SELECT 554, 'Minifigures', '2', 58
UNION SELECT 555, 'Material', 'ABS Plastic', 58
UNION SELECT 556, 'Dimensions', '28x26x6cm', 58
UNION SELECT 557, 'Weight', '0.8kg', 58
UNION SELECT 558, 'Skills', 'Building, Creativity', 58
UNION SELECT 559, 'Board Material', 'Walnut & Maple', 59
UNION SELECT 560, 'Piece Material', 'Weighted Wood', 59
UNION SELECT 561, 'King Height', '8.5cm', 59
UNION SELECT 562, 'Board Size', '45cm x 45cm', 59
UNION SELECT 563, 'Storage', 'Box Included', 59
UNION SELECT 564, 'Skill Level', 'All Levels', 59
UNION SELECT 565, 'Age Range', '8+', 59
UNION SELECT 566, 'Game Type', 'Strategy', 59
UNION SELECT 567, 'Weight', '2.2kg', 59
UNION SELECT 568, 'Pieces', '32', 59
UNION SELECT 569, 'Adjustable Height', '4ft - 6ft', 60
UNION SELECT 570, 'Rim Type', 'Breakaway', 60
UNION SELECT 571, 'Base', 'Fillable with Sand/Water', 60
UNION SELECT 572, 'Included Ball', 'Foam Basketball', 60
UNION SELECT 573, 'Material', 'Plastic & Steel', 60
UNION SELECT 574, 'Indoor/Outdoor', 'Indoor', 60
UNION SELECT 575, 'Age Range', '3-8 years', 60
UNION SELECT 576, 'Assembly Required', 'Yes', 60
UNION SELECT 577, 'Skills', 'Motor Skills', 60
GO;

INSERT INTO Review (Id, [Subject], Rating, Comment, [Status], ProductId, CustomerId, CreatedAt)
SELECT 1, 'Highly recommend', 2, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 1, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 2, 'Wouldnt buy again', 1, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 1, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 3, 'Wouldnt buy again', 2, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 2, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 4, 'Excellent choice!', 4, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 2, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 5, 'Pretty average', 5, 'Its decent but nothing special, kind of average overall.', 'Approved', 3, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 6, 'Fantastic value', 4, 'Affordable, practical, and works as described. Great deal!', 'Approved', 3, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 7, 'Disappointed', 2, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 4, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 8, 'Perfect for daily use', 5, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 4, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 9, 'Not great', 1, 'The quality was okay, but not worth the cost in my opinion.', 'Approved', 5, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 10, 'Exceeded expectations', 2, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 5, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 11, 'Exceeded expectations', 4, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 6, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 12, 'Wouldnt buy again', 2, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 6, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 13, 'Worth every penny', 4, 'Really good build and works perfectly — happy with this purchase!', 'Approved', 7, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 14, 'Fantastic value', 5, 'Affordable, practical, and works as described. Great deal!', 'Approved', 7, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 15, 'Not great', 2, 'The quality was okay, but not worth the cost in my opinion.', 'Approved', 8, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 16, 'Not great', 2, 'The quality was okay, but not worth the cost in my opinion.', 'Approved', 8, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 17, 'Perfect for daily use', 2, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 9, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 18, 'Worth every penny', 3, 'Really good build and works perfectly — happy with this purchase!', 'Approved', 9, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 19, 'Pretty average', 3, 'Its decent but nothing special, kind of average overall.', 'Approved', 10, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 20, 'Excellent choice!', 3, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 10, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 21, 'Exceeded expectations', 3, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 11, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 22, 'Disappointed', 3, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 11, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 23, 'Perfect for daily use', 3, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 12, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 24, 'Wouldnt buy again', 4, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 12, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 25, 'Pretty average', 2, 'Its decent but nothing special, kind of average overall.', 'Approved', 13, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 26, 'Highly recommend', 4, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 13, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 27, 'Fantastic value', 2, 'Affordable, practical, and works as described. Great deal!', 'Approved', 14, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 28, 'Disappointed', 2, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 14, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 29, 'Pretty average', 1, 'Its decent but nothing special, kind of average overall.', 'Approved', 15, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 30, 'Exceeded expectations', 5, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 15, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 31, 'Wouldnt buy again', 4, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 16, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 32, 'Exceeded expectations', 5, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 16, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 33, 'Fantastic value', 1, 'Affordable, practical, and works as described. Great deal!', 'Approved', 17, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 34, 'Highly recommend', 5, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 17, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 35, 'Highly recommend', 1, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 18, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 36, 'Pretty average', 3, 'Its decent but nothing special, kind of average overall.', 'Approved', 18, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 37, 'Excellent choice!', 5, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 19, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 38, 'Not great', 2, 'The quality was okay, but not worth the cost in my opinion.', 'Approved', 19, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 39, 'Disappointed', 4, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 20, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 40, 'Perfect for daily use', 5, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 20, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 41, 'Highly recommend', 2, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 21, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 42, 'Exceeded expectations', 2, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 21, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 43, 'Perfect for daily use', 1, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 22, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 44, 'Highly recommend', 4, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 22, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 45, 'Exceeded expectations', 3, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 23, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 46, 'Highly recommend', 3, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 23, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 47, 'Pretty average', 4, 'Its decent but nothing special, kind of average overall.', 'Approved', 24, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 48, 'Perfect for daily use', 5, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 24, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 49, 'Exceeded expectations', 1, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 25, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 50, 'Fantastic value', 4, 'Affordable, practical, and works as described. Great deal!', 'Approved', 25, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 51, 'Disappointed', 3, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 26, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 52, 'Excellent choice!', 5, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 26, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 53, 'Highly recommend', 5, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 27, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 54, 'Excellent choice!', 1, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 27, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 55, 'Exceeded expectations', 5, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 28, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 56, 'Worth every penny', 3, 'Really good build and works perfectly — happy with this purchase!', 'Approved', 28, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 57, 'Excellent choice!', 4, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 29, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 58, 'Wouldnt buy again', 5, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 29, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 59, 'Exceeded expectations', 2, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 30, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 60, 'Highly recommend', 3, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 30, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 61, 'Highly recommend', 5, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 31, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 62, 'Exceeded expectations', 3, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 31, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 63, 'Fantastic value', 2, 'Affordable, practical, and works as described. Great deal!', 'Approved', 32, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 64, 'Excellent choice!', 5, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 32, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 65, 'Not great', 1, 'The quality was okay, but not worth the cost in my opinion.', 'Approved', 33, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 66, 'Highly recommend', 2, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 33, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 67, 'Perfect for daily use', 4, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 34, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 68, 'Not great', 3, 'The quality was okay, but not worth the cost in my opinion.', 'Approved', 34, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 69, 'Worth every penny', 3, 'Really good build and works perfectly — happy with this purchase!', 'Approved', 35, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 70, 'Excellent choice!', 2, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 35, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 71, 'Highly recommend', 2, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 36, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 72, 'Highly recommend', 1, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 36, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 73, 'Exceeded expectations', 1, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 37, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 74, 'Wouldnt buy again', 3, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 37, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 75, 'Wouldnt buy again', 3, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 38, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 76, 'Perfect for daily use', 4, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 38, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 77, 'Wouldnt buy again', 5, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 39, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 78, 'Disappointed', 3, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 39, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 79, 'Disappointed', 4, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 40, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 80, 'Worth every penny', 5, 'Really good build and works perfectly — happy with this purchase!', 'Approved', 40, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 81, 'Fantastic value', 2, 'Affordable, practical, and works as described. Great deal!', 'Approved', 41, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 82, 'Perfect for daily use', 2, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 41, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 83, 'Exceeded expectations', 5, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 42, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 84, 'Excellent choice!', 5, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 42, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 85, 'Excellent choice!', 1, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 43, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 86, 'Disappointed', 4, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 43, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 87, 'Pretty average', 4, 'Its decent but nothing special, kind of average overall.', 'Approved', 44, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 88, 'Wouldnt buy again', 2, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 44, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 89, 'Disappointed', 1, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 45, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 90, 'Pretty average', 4, 'Its decent but nothing special, kind of average overall.', 'Approved', 45, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 91, 'Exceeded expectations', 5, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 46, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 92, 'Disappointed', 5, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 46, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 93, 'Wouldnt buy again', 2, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 47, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 94, 'Not great', 2, 'The quality was okay, but not worth the cost in my opinion.', 'Approved', 47, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 95, 'Highly recommend', 3, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 48, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 96, 'Highly recommend', 4, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 48, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 97, 'Exceeded expectations', 1, 'I was surprised at how sturdy and reliable this turned out to be.', 'Approved', 49, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 98, 'Worth every penny', 1, 'Really good build and works perfectly — happy with this purchase!', 'Approved', 49, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 99, 'Excellent choice!', 4, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 50, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 100, 'Disappointed', 3, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 50, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 101, 'Not great', 5, 'The quality was okay, but not worth the cost in my opinion.', 'Approved', 51, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 102, 'Fantastic value', 3, 'Affordable, practical, and works as described. Great deal!', 'Approved', 51, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 103, 'Excellent choice!', 5, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 52, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 104, 'Not great', 3, 'The quality was okay, but not worth the cost in my opinion.', 'Approved', 52, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 105, 'Worth every penny', 5, 'Really good build and works perfectly — happy with this purchase!', 'Approved', 53, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 106, 'Excellent choice!', 5, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 53, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 107, 'Highly recommend', 5, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 54, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 108, 'Perfect for daily use', 5, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 54, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 109, 'Disappointed', 4, 'Unfortunately, the item didnt match the description and felt cheap.', 'Approved', 55, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 110, 'Wouldnt buy again', 4, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 55, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 111, 'Wouldnt buy again', 3, 'Didnt last long before showing wear and tear — not impressed.', 'Approved', 56, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 112, 'Highly recommend', 1, 'Fast shipping and excellent packaging, Ill definitely order from here again.', 'Approved', 56, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 113, 'Worth every penny', 5, 'Really good build and works perfectly — happy with this purchase!', 'Approved', 57, 2, '2024-09-14 22:40:39.7732556'
UNION SELECT 114, 'Worth every penny', 5, 'Really good build and works perfectly — happy with this purchase!', 'Approved', 57, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 115, 'Not great', 5, 'The quality was okay, but not worth the cost in my opinion.', 'Approved', 58, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 116, 'Perfect for daily use', 3, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 58, 3, '2024-09-14 22:40:39.7732556'
UNION SELECT 117, 'Worth every penny', 1, 'Really good build and works perfectly — happy with this purchase!', 'Approved', 59, 5, '2024-09-14 22:40:39.7732556'
UNION SELECT 118, 'Excellent choice!', 3, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 59, 1, '2024-09-14 22:40:39.7732556'
UNION SELECT 119, 'Perfect for daily use', 4, 'Exactly what I needed, Ive been using it every day and it holds up well.', 'Approved', 60, 4, '2024-09-14 22:40:39.7732556'
UNION SELECT 120, 'Excellent choice!', 1, 'The product quality is amazing, much better than I expected for the price.', 'Approved', 60, 1, '2024-09-14 22:40:39.7732556'
GO;