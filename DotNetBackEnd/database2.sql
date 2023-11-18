USE [db-main]
GO
SET IDENTITY_INSERT [dbo].[VIPs] ON 

INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (1, 500000, 999000, 5, 1)
INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (2, 1000000, 1999000, 6, 1)
INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (4, 2000000, 2999000, 7, 1)
INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (6, 3000000, 3999000, 8, 1)
INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (7, 4000000, 4999000, 9, 1)
INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (8, 5000000, 5999000, 10, 1)
INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (9, 6000000, 6999000, 11, 1)
INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (10, 7000000, 7999000, 12, 1)
INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (11, 8000000, 8999000, 13, 1)
INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (12, 9000000, 9999000, 14, 1)
INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (13, 10000000, 0, 15, 1)
SET IDENTITY_INSERT [dbo].[VIPs] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [LoginName], [Name], [TelNum], [PasswordHash], [PasswordSalt], [RefreshToken], [TokenCreated], [TokenExpires], [Email], [Address], [AddressCode], [IsActive], [vipsId]) VALUES (14, N'VQT', N'Võ Quang Tường', N'0339518617', 0xCCF8BB8C7DABD73F40D12FE2B25A3CF6C33A95FEDE41C95AB56ABA4714C14659431E84EDFC2771BABC4DDE9671F506C6538F5637F6FCEC1CD4AF2AD0A29E8DBC, 0x9ABDC28C5AD7CD05243F8EF9BAAACE055366BDF6BF26858792B7174B47C3B2ED87DC9C4DE1C6A5746A9B11D702C2A1E524A63B2598EC5AD357271283230721F1D9AF4649C9FD6516214B50290BBF68F55D8C6CCBAF5E897A40AEFC0F919BBFC02048D0D6B350C536F0B72178DC046FC4F231350E2D212DDEC0194FD104E22BC5, N'agXlZVR0S7HOf29iGtE5ySTFE1be0Zw/nfxfoIIibAmvK9+0e9rlra1s24WqcL6WuCcd7kF1quMDo0Esy2vKKw==', CAST(N'2023-11-18T21:24:14.3231148' AS DateTime2), CAST(N'2023-11-18T21:54:14.3232328' AS DateTime2), N'quangtuong6687@gmail.com', N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', N'26635, 751, 77', 1, NULL)
INSERT [dbo].[Users] ([Id], [LoginName], [Name], [TelNum], [PasswordHash], [PasswordSalt], [RefreshToken], [TokenCreated], [TokenExpires], [Email], [Address], [AddressCode], [IsActive], [vipsId]) VALUES (17, N'taikhoantest', N'TÀI KHOẢN ĐỂ TEST ', N'0347141540', 0xDDAC45037E2BA4348092352A535F102BCB2A81F2299E2B0E1DBE66E0B53487AE4E6CB63D5D12C6BED92B2E64E8241923D88BCF9E2024ED990DB1B80BAD3202DB, 0xB105A03B8F04F2C64278F678AEC6F7543C18C69F88610D094001AB8BED4539B26B013413DA688EB5B67DF3F77E55F8215A7D72F85B60A33AED678E52D106A002C0D91A993B78415D0CC45725E7D9020017E53FFA0D17064A6019B12D260AA003F5F1EE9B900B464B159E43F134B66D57791BA0521C0A2C1F64D53315801293DC, N'mAZU0PyoQqrCKWemBw8wWrmnNBcxHJScqyKtCVcqOLskHdT/KxDp8rqn75V16f7QX2triEKb9W1/5x6jKvVISA==', CAST(N'2023-11-15T16:49:44.0054034' AS DateTime2), CAST(N'2023-11-15T17:19:44.0054051' AS DateTime2), N'quangtuong64@gmail.com', N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', N'26635, 751, 77', 1, NULL)
INSERT [dbo].[Users] ([Id], [LoginName], [Name], [TelNum], [PasswordHash], [PasswordSalt], [RefreshToken], [TokenCreated], [TokenExpires], [Email], [Address], [AddressCode], [IsActive], [vipsId]) VALUES (19, N'LHA', N'Lê Hoàng Anh', N'0339000111', 0x7D6187B9AE0B64F95380338CF114A2D86F9668807078F1607375A5262506BF6E5FAB6F5BB96115BD7F0F0FA269ADA670A42B7B2A3743F74C20102DD619BC1A6D, 0x2E881EC328BB4816A7B27CE4BDE0BD74537A97BACD98E217155E6AA71B6BBD7F671C9528AE9144BC28EC8D36F1BCE6F2AD180AE94EA8DB254047BDEF7CD26568B594E36324AD599391D59D31333129FF2E4EFD5F35B7E4771A20DC99BB2E2237108FF206061865A2BE69FD00A2791A817B09681A08D5C7FAEFCBE2FD3116CD1B, N'mb5KLxSO3uVBNOK9UcknpUTVgv08mwQARAzOE3D9uHGzZedjgdL5H7V2NqswRiHmqkZuNdR7FKe58Bro1/zMgQ==', CAST(N'2023-11-18T14:25:17.5286236' AS DateTime2), CAST(N'2023-11-18T14:55:17.5286248' AS DateTime2), N'quangtuong100@gmail.com', N'279 Bưng Ông Thoàn, Phường Tăng Nhơn Phú B, Thành phố Thủ Đức, Thành phố Hồ Chí Minh', N'26845, 769, 79', 1, 1)
INSERT [dbo].[Users] ([Id], [LoginName], [Name], [TelNum], [PasswordHash], [PasswordSalt], [RefreshToken], [TokenCreated], [TokenExpires], [Email], [Address], [AddressCode], [IsActive], [vipsId]) VALUES (21, N'qwerty', N'Võ Quang Tường', N'0123456789', 0x135661CE7EEC7031E2A40DB23E34B20646D6F77BFB06A970A0AB04B24F72A769B28838C9761E36770189243EA68BE9687ACB01D3F7445A4E3052D300096E5173, 0x5901779742029525F1314A6FC3DC2D5649DC3B8A784FFC3DA9EAE30C2831AA3CFCE1B5A33C11B036AA8669AB45858EB07C833F3010D5EC2BAEE13A5E0B7681AFFD9CF1F0324192C23D7E6DDBB6415E979114B4E406EE579C65091546864D33F9C20C592EF5A610B4D2B386F766BB7469FEE538C13D44B346F28D047E894134AA, N'KipEI0febljK3Skq7bExke2kgaQZpGbY54OhsQjsjezd9NFVaOo4RtqIEIs1/GvLt4cwPYfOftlQQ2+mWrJhFw==', CAST(N'2023-10-29T01:43:43.7738583' AS DateTime2), CAST(N'2023-10-29T02:13:43.7738595' AS DateTime2), N'quangtuong102@gmail.com', N'1czx, Phường Phúc Xá, Quận Ba Đình, Thành phố Hà Nội', N'1, 1, 1', 1, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[Brands] ON 

INSERT [dbo].[Brands] ([Id], [BrandName], [Description], [IsActive]) VALUES (1, N'Thiên Longg', N'Nhà sản xuất bút số 1 Việt Nam', 1)
INSERT [dbo].[Brands] ([Id], [BrandName], [Description], [IsActive]) VALUES (2, N'Bến Nghé', N'Nhà sản xuất giấy số 2 không ai số 1...', 1)
INSERT [dbo].[Brands] ([Id], [BrandName], [Description], [IsActive]) VALUES (3, N'Paper Mate', N'Nhãn hàng sản xuất thủ thứ', 1)
INSERT [dbo].[Brands] ([Id], [BrandName], [Description], [IsActive]) VALUES (4, N'Casio', N'Nhãn hàng sản xuất máy tính cầm tay ', 1)
INSERT [dbo].[Brands] ([Id], [BrandName], [Description], [IsActive]) VALUES (5, N'Campus', N'Nhãn hàng sản xuất vở ghi chất lượng cao', 1)
INSERT [dbo].[Brands] ([Id], [BrandName], [Description], [IsActive]) VALUES (6, N'Flexio', N'Máy tính khoa học', 1)
INSERT [dbo].[Brands] ([Id], [BrandName], [Description], [IsActive]) VALUES (7, N'Mr.Bean', N'Thương hiệu đến từ ĐứcC', 1)
INSERT [dbo].[Brands] ([Id], [BrandName], [Description], [IsActive]) VALUES (8, N'Nhãn hàng test', NULL, 0)
INSERT [dbo].[Brands] ([Id], [BrandName], [Description], [IsActive]) VALUES (9, N'Nhãn hàng test 1   ', NULL, 0)
SET IDENTITY_INSERT [dbo].[Brands] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [CategoryName], [IsActive], [Description], [Image]) VALUES (7, N'Bút các loạii', 1, NULL, N'Resources\Images\image_217001.jpg')
INSERT [dbo].[Categories] ([Id], [CategoryName], [IsActive], [Description], [Image]) VALUES (8, N'Máy tính điện tử', 1, NULL, N'Resources\Images\4549526604379-_6_.jpg')
INSERT [dbo].[Categories] ([Id], [CategoryName], [IsActive], [Description], [Image]) VALUES (9, N'Vở ', 1, NULL, N'Resources\Images\8934986000193_4.jpg')
INSERT [dbo].[Categories] ([Id], [CategoryName], [IsActive], [Description], [Image]) VALUES (11, N'Nhãn hàng test', 1, NULL, N'Resources\Images\Screenshot 2023-03-19 132452.png')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Id], [ProductName], [Description], [Image], [Price], [Discount], [Quanity], [SoldQuantity], [CategoryId], [BrandId], [CreatedDate], [IsActive], [UpdatedDate], [ImageDetail]) VALUES (11, N'Bút Bi 6 Màu Trái Thơm', N'Thì là bút bi thôi chứ mô tả gì nữa =))', N'Resources\Images\6949029955899-_6_.jpg', 10000, 20, 42, 2, 7, 2, CAST(N'2023-08-10T10:12:35.2502492+00:00' AS DateTimeOffset), 1, CAST(N'2023-08-10T10:12:35.2502522+00:00' AS DateTimeOffset), N'Resources\Images\4549526604379-_6_.jpg,Resources\Images\Screenshot 2023-03-19 132452.png,Resources\Images\8935001804086.jpg')
INSERT [dbo].[Products] ([Id], [ProductName], [Description], [Image], [Price], [Discount], [Quanity], [SoldQuantity], [CategoryId], [BrandId], [CreatedDate], [IsActive], [UpdatedDate], [ImageDetail]) VALUES (12, N'Máy Tính Văn Phòng Casio MP12R', N'Máy tính cầm tay nhỏ gọn, tiện ích', N'Resources\Images\4549526604379-_6_.jpg', 699000, 25, 1, 3, 8, 4, CAST(N'2023-09-23T12:58:41.4937209+00:00' AS DateTimeOffset), 1, CAST(N'2023-09-23T12:58:41.4937215+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Products] ([Id], [ProductName], [Description], [Image], [Price], [Discount], [Quanity], [SoldQuantity], [CategoryId], [BrandId], [CreatedDate], [IsActive], [UpdatedDate], [ImageDetail]) VALUES (13, N'Vở doraemon', N'Tập 4 Ly Ngang 200 Trang', N'Resources\Images\8936038723180-mau1_3__1.jpg', 100001, 10, 8, 3, 9, 5, CAST(N'2023-08-09T19:56:58.4858639+00:00' AS DateTimeOffset), 1, CAST(N'2023-08-09T19:56:58.4858642+00:00' AS DateTimeOffset), N'Resources/Images/ddb8e14b6e3a2db62dc67214c6a1c4c7.jpg_720x720q80.jpg_.webp')
INSERT [dbo].[Products] ([Id], [ProductName], [Description], [Image], [Price], [Discount], [Quanity], [SoldQuantity], [CategoryId], [BrandId], [CreatedDate], [IsActive], [UpdatedDate], [ImageDetail]) VALUES (14, N'Tập 4 Ly Ngang 96 Trang', N'Tập 4 Ly Ngang 96 Trang ĐL 70g/m2', N'Resources\Images\8934986000193_4.jpg', 20000, 20, 1, 2, 9, 3, CAST(N'2023-08-09T19:58:45.0482316+00:00' AS DateTimeOffset), 1, CAST(N'2023-08-09T19:58:45.0482321+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Products] ([Id], [ProductName], [Description], [Image], [Price], [Discount], [Quanity], [SoldQuantity], [CategoryId], [BrandId], [CreatedDate], [IsActive], [UpdatedDate], [ImageDetail]) VALUES (15, N'Máy Tính Flexio Fx799VN ', N'Máy Tính Khoa Học Flexio - Thiên Long Fx799VN - Màu Đen', N'Resources\Images\8935324005702.jpg', 559000, 66, 18, 1, 8, 6, CAST(N'2023-08-09T20:12:18.4473827+00:00' AS DateTimeOffset), 1, CAST(N'2023-08-09T20:12:18.4473829+00:00' AS DateTimeOffset), NULL)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [Name]) VALUES (1, N'Normal')
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (2, N'Manager')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (14, 1, 1)
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (14, 2, 1)
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (17, 1, 1)
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (19, 1, 1)
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (19, 2, 0)
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (21, 1, 1)
GO
INSERT [dbo].[Carts] ([UserId], [ProductId], [Quantity], [CreatedDate], [UpdatedDate]) VALUES (14, 12, 4, CAST(N'2023-09-23T12:45:34.7648373+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Carts] ([UserId], [ProductId], [Quantity], [CreatedDate], [UpdatedDate]) VALUES (17, 11, 1, CAST(N'2023-11-15T16:12:04.5627269+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Carts] ([UserId], [ProductId], [Quantity], [CreatedDate], [UpdatedDate]) VALUES (17, 14, 1, CAST(N'2023-11-15T16:50:33.5895525+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Carts] ([UserId], [ProductId], [Quantity], [CreatedDate], [UpdatedDate]) VALUES (19, 12, 1, CAST(N'2023-10-28T09:55:34.2449804+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Carts] ([UserId], [ProductId], [Quantity], [CreatedDate], [UpdatedDate]) VALUES (19, 13, 5, CAST(N'2023-10-28T09:55:40.0230874+00:00' AS DateTimeOffset), CAST(N'2023-11-15T16:03:56.7052542+00:00' AS DateTimeOffset))
INSERT [dbo].[Carts] ([UserId], [ProductId], [Quantity], [CreatedDate], [UpdatedDate]) VALUES (19, 14, 1, CAST(N'2023-11-15T16:04:11.1778090+00:00' AS DateTimeOffset), NULL)
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (31, 14, N'Success', 14, N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 990000, N'A', CAST(N'2023-08-07T15:19:45.9493619+00:00' AS DateTimeOffset), 1, NULL, 0)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (32, 17, N'Cancel', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 1106500, N'A', CAST(N'2023-08-10T10:04:03.0710132+00:00' AS DateTimeOffset), 1, NULL, 0)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (38, 17, N'Cancel', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 1164500, N'A', CAST(N'2023-08-10T17:02:43.3673633+00:00' AS DateTimeOffset), 1, NULL, 0)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (39, 14, N'Pending', 14, N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 8000, N'A', CAST(N'2023-08-10T17:07:54.4729639+00:00' AS DateTimeOffset), 1, NULL, 0)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (40, 14, N'Pending', 14, N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 524250, N'A', CAST(N'2023-08-10T17:11:55.4854018+00:00' AS DateTimeOffset), 1, NULL, 0)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (41, 14, N'Pending', 14, N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 524250, N'A', CAST(N'2023-08-10T17:17:42.3342534+00:00' AS DateTimeOffset), 1, NULL, 0)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (42, 17, N'Cancel', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 240060, N'A', CAST(N'2023-08-10T17:22:46.4920694+00:00' AS DateTimeOffset), 1, NULL, 0)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (43, 17, N'Success', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 524250, N'A', CAST(N'2023-08-10T17:23:17.4424117+00:00' AS DateTimeOffset), 1, NULL, 0)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (44, 17, N'Success', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 574250, N'A', CAST(N'2023-08-10T17:32:17.0829635+00:00' AS DateTimeOffset), 1, NULL, 0)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (46, 14, N'Cancel', 14, N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 3145500, N'A', CAST(N'2023-09-23T12:42:18.1090671+00:00' AS DateTimeOffset), 1, NULL, 0)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (1046, 19, N'Success', 19, N'279 Bưng Ông Thoàn, Phường Tăng Nhơn Phú B, Thành phố Thủ Đức, Thành phố Hồ Chí Minh', 322060, N'A', CAST(N'2023-10-22T06:10:19.4664733+00:00' AS DateTimeOffset), 1, NULL, 5)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP]) VALUES (1047, 17, N'Processing', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 1048500, N'A', CAST(N'2023-11-07T17:13:16.0830047+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-18T14:24:44.1227288+00:00' AS DateTimeOffset), 0)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
INSERT [dbo].[Feedbacks] ([UserId], [ProductId], [Comments], [Votes], [OrderId], [CreatedDate], [IsActive], [UpdatedDate], [AdminReply], [ReplyDate]) VALUES (17, 12, N'không tốt', 1, 44, CAST(N'2023-11-13T14:51:01.8323658+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-13T15:41:48.6654457+00:00' AS DateTimeOffset), NULL, NULL)
INSERT [dbo].[Feedbacks] ([UserId], [ProductId], [Comments], [Votes], [OrderId], [CreatedDate], [IsActive], [UpdatedDate], [AdminReply], [ReplyDate]) VALUES (17, 13, N'tốt lắm', 5, 44, CAST(N'2023-11-13T14:51:01.8325217+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-13T15:41:48.6654479+00:00' AS DateTimeOffset), N'cám ơn bạn', CAST(N'2023-11-13T15:45:32.9365627+00:00' AS DateTimeOffset))
INSERT [dbo].[Feedbacks] ([UserId], [ProductId], [Comments], [Votes], [OrderId], [CreatedDate], [IsActive], [UpdatedDate], [AdminReply], [ReplyDate]) VALUES (19, 13, N'tốt ', 5, 1046, CAST(N'2023-11-13T15:42:21.8404173+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-13T15:42:21.8404174+00:00' AS DateTimeOffset), NULL, NULL)
INSERT [dbo].[Feedbacks] ([UserId], [ProductId], [Comments], [Votes], [OrderId], [CreatedDate], [IsActive], [UpdatedDate], [AdminReply], [ReplyDate]) VALUES (19, 14, NULL, 1, 1046, CAST(N'2023-11-13T15:42:21.8404184+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-13T15:42:21.8404184+00:00' AS DateTimeOffset), NULL, NULL)
INSERT [dbo].[Feedbacks] ([UserId], [ProductId], [Comments], [Votes], [OrderId], [CreatedDate], [IsActive], [UpdatedDate], [AdminReply], [ReplyDate]) VALUES (19, 15, NULL, 1, 1046, CAST(N'2023-11-13T15:42:21.8404186+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-13T15:42:21.8404186+00:00' AS DateTimeOffset), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 

INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (54, 11, 1000000, 31, 2, 20)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (55, 12, 1000000, 31, 1, 25)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (56, 11, 10000, 32, 1, 20)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (57, 12, 699000, 32, 2, 25)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (58, 13, 100000, 32, 1, 50)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (74, 11, 10000, 38, 2, 20)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (75, 12, 699000, 38, 2, 25)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (76, 13, 100000, 38, 2, 50)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (77, 11, 10000, 39, 1, 20)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (78, 12, 699000, 40, 1, 25)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (79, 12, 699000, 41, 1, 25)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (80, 13, 100000, 42, 1, 50)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (81, 15, 559000, 42, 1, 66)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (82, 12, 699000, 43, 1, 25)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (83, 12, 699000, 44, 1, 25)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (84, 13, 100000, 44, 1, 50)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (88, 12, 699000, 46, 6, 25)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1088, 13, 100000, 1046, 2, 50)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1089, 14, 20000, 1046, 2, 20)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1090, 15, 559000, 1046, 1, 66)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1091, 12, 699000, 1047, 2, 25)
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
