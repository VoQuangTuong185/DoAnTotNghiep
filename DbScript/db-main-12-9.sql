USE [db-main]
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

INSERT [dbo].[Categories] ([Id], [CategoryName], [IsActive], [Description], [Image]) VALUES (7, N'Bút các loại', 1, NULL, N'Resources/Images/but-xoa-dung-cu-huu-ich-cuu-canh-cua-dan-van-phong-2.jpg')
INSERT [dbo].[Categories] ([Id], [CategoryName], [IsActive], [Description], [Image]) VALUES (8, N'Máy tính điện tử', 1, N'Máy tính điện tử các loại', N'Resources\Images\4549526604379-_6_.jpg')
INSERT [dbo].[Categories] ([Id], [CategoryName], [IsActive], [Description], [Image]) VALUES (9, N'Vở ', 1, N'MO TA', N'Resources\Images\8934986000193_4.jpg')
INSERT [dbo].[Categories] ([Id], [CategoryName], [IsActive], [Description], [Image]) VALUES (11, N'Nhãn hàng test', 0, NULL, N'Resources\Images\Screenshot 2023-03-19 132452.png')
INSERT [dbo].[Categories] ([Id], [CategoryName], [IsActive], [Description], [Image]) VALUES (12, N'test', 1, N'test', N'Resources\Images\8935337501482.jpg')
INSERT [dbo].[Categories] ([Id], [CategoryName], [IsActive], [Description], [Image]) VALUES (13, N'test1', 1, NULL, N'Resources/Images/WALLPAPER.jpg')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Id], [ProductName], [Description], [Image], [Price], [Discount], [Quanity], [SoldQuantity], [CategoryId], [BrandId], [CreatedDate], [IsActive], [UpdatedDate], [ImageDetail]) VALUES (11, N'Bút Bi 6 Màu Trái Thơm', N'Thì là bút bi thôi chứ mô tả gì nữa =))', N'Resources\Images\6949029955899-_6_.jpg', 10000, 20, 39, 3, 7, 2, CAST(N'2023-08-10T10:12:35.2502492+00:00' AS DateTimeOffset), 1, CAST(N'2023-08-10T10:12:35.2502522+00:00' AS DateTimeOffset), N'Resources\Images\4549526604379-_6_.jpg,Resources\Images\Screenshot 2023-03-19 132452.png,Resources\Images\8935001804086.jpg')
INSERT [dbo].[Products] ([Id], [ProductName], [Description], [Image], [Price], [Discount], [Quanity], [SoldQuantity], [CategoryId], [BrandId], [CreatedDate], [IsActive], [UpdatedDate], [ImageDetail]) VALUES (12, N'Máy Tính Văn Phòng Casio MP12R', N'Máy tính cầm tay nhỏ gọn, tiện ích', N'Resources\Images\4549526604379-_6_.jpg', 699000, 25, 0, 7, 8, 4, CAST(N'2023-09-23T12:58:41.4937209+00:00' AS DateTimeOffset), 1, CAST(N'2023-09-23T12:58:41.4937215+00:00' AS DateTimeOffset), N'Resources\Images\4971850091431.jpg')
INSERT [dbo].[Products] ([Id], [ProductName], [Description], [Image], [Price], [Discount], [Quanity], [SoldQuantity], [CategoryId], [BrandId], [CreatedDate], [IsActive], [UpdatedDate], [ImageDetail]) VALUES (13, N'Vở doraemonn', N'Tập 4 Ly Ngang 200 Trang', N'Resources\Images\8936038723180-mau1_3__1.jpg', 100001, 10, 0, 3, 9, 5, CAST(N'2023-08-09T19:56:58.4858639+00:00' AS DateTimeOffset), 0, CAST(N'2023-08-09T19:56:58.4858642+00:00' AS DateTimeOffset), N'Resources/Images/ddb8e14b6e3a2db62dc67214c6a1c4c7.jpg_720x720q80.jpg_.webp')
INSERT [dbo].[Products] ([Id], [ProductName], [Description], [Image], [Price], [Discount], [Quanity], [SoldQuantity], [CategoryId], [BrandId], [CreatedDate], [IsActive], [UpdatedDate], [ImageDetail]) VALUES (14, N'Tập 4 Ly Ngang 96 Trang', N'Tập 4 Ly Ngang 96 Trang ĐL 70g/m2', N'Resources\Images\8934986000193_4.jpg', 30000, 20, 2, 3, 9, 3, CAST(N'2023-08-09T19:58:45.0482316+00:00' AS DateTimeOffset), 1, CAST(N'2023-08-09T19:58:45.0482321+00:00' AS DateTimeOffset), N'Resources/Images/ddb8e14b6e3a2db62dc67214c6a1c4c7.jpg_720x720q80.jpg_.webp')
INSERT [dbo].[Products] ([Id], [ProductName], [Description], [Image], [Price], [Discount], [Quanity], [SoldQuantity], [CategoryId], [BrandId], [CreatedDate], [IsActive], [UpdatedDate], [ImageDetail]) VALUES (15, N'Máy Tính Flexio Fx799VN ', N'Máy Tính Khoa Học Flexio - Thiên Long Fx799VN - Màu Đen', N'Resources\Images\8935324005702.jpg', 559000, 66, 18, 1, 8, 6, CAST(N'2023-08-09T20:12:18.4473827+00:00' AS DateTimeOffset), 1, CAST(N'2023-08-09T20:12:18.4473829+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Products] ([Id], [ProductName], [Description], [Image], [Price], [Discount], [Quanity], [SoldQuantity], [CategoryId], [BrandId], [CreatedDate], [IsActive], [UpdatedDate], [ImageDetail]) VALUES (18, N'test', N'test', N'Resources\Images\Screenshot 2023-05-14 231441.png', 100000, 10, 3, 0, 9, 4, CAST(N'2023-11-21T16:17:08.1288604+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-21T16:17:08.1288612+00:00' AS DateTimeOffset), NULL)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [Name]) VALUES (1, N'Normal')
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (2, N'Manager')
SET IDENTITY_INSERT [dbo].[Roles] OFF
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
INSERT [dbo].[VIPs] ([Id], [PriceFrom], [PriceTo], [Discount], [IsActive]) VALUES (14, 0, 499000, 0, 1)
SET IDENTITY_INSERT [dbo].[VIPs] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [LoginName], [Name], [TelNum], [PasswordHash], [PasswordSalt], [RefreshToken], [TokenCreated], [TokenExpires], [Email], [Address], [AddressCode], [IsActive], [VipsId]) VALUES (14, N'VQT', N'Võ Quang Tường', N'0339518617', 0xCCF8BB8C7DABD73F40D12FE2B25A3CF6C33A95FEDE41C95AB56ABA4714C14659431E84EDFC2771BABC4DDE9671F506C6538F5637F6FCEC1CD4AF2AD0A29E8DBC, 0x9ABDC28C5AD7CD05243F8EF9BAAACE055366BDF6BF26858792B7174B47C3B2ED87DC9C4DE1C6A5746A9B11D702C2A1E524A63B2598EC5AD357271283230721F1D9AF4649C9FD6516214B50290BBF68F55D8C6CCBAF5E897A40AEFC0F919BBFC02048D0D6B350C536F0B72178DC046FC4F231350E2D212DDEC0194FD104E22BC5, N'B9WqszoJ8mH9ylL2yVNY0PNfGdoGuSutdHfl0C0FDVjSB5c50SaKpOgaIDV3gowtff42IhEVsS/fLEpv8GKzcQ==', CAST(N'2023-12-03T07:45:19.7652470' AS DateTime2), CAST(N'2023-12-03T08:15:19.7652489' AS DateTime2), N'quangtuong6687@gmail.com', N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', N'26635, 751, 77', 1, 1)
INSERT [dbo].[Users] ([Id], [LoginName], [Name], [TelNum], [PasswordHash], [PasswordSalt], [RefreshToken], [TokenCreated], [TokenExpires], [Email], [Address], [AddressCode], [IsActive], [VipsId]) VALUES (17, N'taikhoantest', N'TÀI KHOẢN ĐỂ TEST ', N'0347141540', 0xDDAC45037E2BA4348092352A535F102BCB2A81F2299E2B0E1DBE66E0B53487AE4E6CB63D5D12C6BED92B2E64E8241923D88BCF9E2024ED990DB1B80BAD3202DB, 0xB105A03B8F04F2C64278F678AEC6F7543C18C69F88610D094001AB8BED4539B26B013413DA688EB5B67DF3F77E55F8215A7D72F85B60A33AED678E52D106A002C0D91A993B78415D0CC45725E7D9020017E53FFA0D17064A6019B12D260AA003F5F1EE9B900B464B159E43F134B66D57791BA0521C0A2C1F64D53315801293DC, N'Ol1lvGsOcVT+dF1ECwLTZ2Oh6iGNGu/kjZ5D64IzqgPQYGetF2Krd2WgilhpOmvNx4ErXn1hUMoNXYtPXjuhJA==', CAST(N'2023-12-03T07:27:45.5979426' AS DateTime2), CAST(N'2023-12-03T07:57:45.5979446' AS DateTime2), N'quangtuong64@gmail.com', N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', N'26635, 751, 77', 1, 6)
INSERT [dbo].[Users] ([Id], [LoginName], [Name], [TelNum], [PasswordHash], [PasswordSalt], [RefreshToken], [TokenCreated], [TokenExpires], [Email], [Address], [AddressCode], [IsActive], [VipsId]) VALUES (19, N'LHA', N'Lê Hoàng Anh', N'0339000111', 0x7D6187B9AE0B64F95380338CF114A2D86F9668807078F1607375A5262506BF6E5FAB6F5BB96115BD7F0F0FA269ADA670A42B7B2A3743F74C20102DD619BC1A6D, 0x2E881EC328BB4816A7B27CE4BDE0BD74537A97BACD98E217155E6AA71B6BBD7F671C9528AE9144BC28EC8D36F1BCE6F2AD180AE94EA8DB254047BDEF7CD26568B594E36324AD599391D59D31333129FF2E4EFD5F35B7E4771A20DC99BB2E2237108FF206061865A2BE69FD00A2791A817B09681A08D5C7FAEFCBE2FD3116CD1B, N'4bt0Vn98gogU0ZQGkKUt5KSntbQa0l+WmdnMfCThsSLQwqrGs4P1LCGazSS7jshT4C14o/AD3zkVaW5Nty6xTg==', CAST(N'2023-12-03T05:58:04.7304580' AS DateTime2), CAST(N'2023-12-03T06:28:04.7304599' AS DateTime2), N'quangtuong100@gmail.com', N'279 Bưng Ông Thoàn, Phường Tăng Nhơn Phú B, Thành phố Thủ Đức, Thành phố Hồ Chí Minh', N'26845, 769, 79', 1, 1)
INSERT [dbo].[Users] ([Id], [LoginName], [Name], [TelNum], [PasswordHash], [PasswordSalt], [RefreshToken], [TokenCreated], [TokenExpires], [Email], [Address], [AddressCode], [IsActive], [VipsId]) VALUES (29, N'VQT1HC', N'Võ Quang Tường', N'0339518618', 0x6EE8FFCFC2618373963026547B0536CC8268CD798A6B9A561A4396C5DFD4A3962AC954CD3030CA87BF1074EA76B5DA474D2C374D905D0FEB6D8B236E6C1C1F24, 0x29AE4FA22F2F4145169665DA4E11AE0C5482408CFB88B4FB2FD64B9E1E1DD287E651D27A550A512C39F5C8EDB6721F8F391FDE1F65F6F878D5EF90F80AD36D156BAB700201DC35A887848E98ACA0E792CA82C41DF5CB2974358F5ED3D41C19B3717B82CFC3816A9B1590C1FD5A3C115766F7717931DCCD3E2E2FED9E6B8EC962, N'OA6DwNlEH0ZwOF+qHqI8a0OyVWZPPkaECmmnngnHXkdAa2vwhxjjWUx6IJxUPdSQv3sIhLRZGidOX5m0OfnsOA==', CAST(N'2023-12-03T05:29:48.3344879' AS DateTime2), CAST(N'2023-12-03T05:59:48.3344900' AS DateTime2), N'n19dccn185@student.ptithcm.edu.vn', N'97 Man Thiện, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', N'26635, 751, 77', 1, 14)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (14, 1, 1)
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (14, 2, 1)
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (17, 1, 1)
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (17, 2, 0)
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (19, 1, 1)
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (19, 2, 0)
INSERT [dbo].[UserAPIs] ([UserId], [RoleId], [IsActive]) VALUES (29, 1, 1)
GO
INSERT [dbo].[Carts] ([UserId], [ProductId], [Quantity], [CreatedDate], [UpdatedDate]) VALUES (14, 12, 4, CAST(N'2023-09-23T12:45:34.7648373+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Carts] ([UserId], [ProductId], [Quantity], [CreatedDate], [UpdatedDate]) VALUES (17, 13, 1, CAST(N'2023-12-03T07:34:10.8192254+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Carts] ([UserId], [ProductId], [Quantity], [CreatedDate], [UpdatedDate]) VALUES (19, 12, 2, CAST(N'2023-10-28T09:55:34.2449804+00:00' AS DateTimeOffset), CAST(N'2023-11-25T04:48:38.0878253+00:00' AS DateTimeOffset))
INSERT [dbo].[Carts] ([UserId], [ProductId], [Quantity], [CreatedDate], [UpdatedDate]) VALUES (19, 13, 4, CAST(N'2023-10-28T09:55:40.0230874+00:00' AS DateTimeOffset), CAST(N'2023-11-15T16:03:56.7052542+00:00' AS DateTimeOffset))
INSERT [dbo].[Carts] ([UserId], [ProductId], [Quantity], [CreatedDate], [UpdatedDate]) VALUES (19, 14, 1, CAST(N'2023-11-15T16:04:11.1778090+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Feedbacks] ([UserId], [ProductId], [Comments], [Votes], [OrderId], [CreatedDate], [IsActive], [UpdatedDate], [AdminReply], [ReplyDate]) VALUES (17, 12, N'ok', 3, 43, CAST(N'2023-12-03T06:16:27.2988750+00:00' AS DateTimeOffset), 1, CAST(N'2023-12-03T06:16:27.2988764+00:00' AS DateTimeOffset), NULL, NULL)
INSERT [dbo].[Feedbacks] ([UserId], [ProductId], [Comments], [Votes], [OrderId], [CreatedDate], [IsActive], [UpdatedDate], [AdminReply], [ReplyDate]) VALUES (17, 12, N'không tốt', 1, 44, CAST(N'2023-11-13T14:51:01.8323658+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-13T15:41:48.6654457+00:00' AS DateTimeOffset), NULL, NULL)
INSERT [dbo].[Feedbacks] ([UserId], [ProductId], [Comments], [Votes], [OrderId], [CreatedDate], [IsActive], [UpdatedDate], [AdminReply], [ReplyDate]) VALUES (17, 13, N'tốt lắm', 5, 44, CAST(N'2023-11-13T14:51:01.8325217+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-13T15:41:48.6654479+00:00' AS DateTimeOffset), N'cám ơn bạn hehe', CAST(N'2023-12-02T04:23:40.3832391+00:00' AS DateTimeOffset))
INSERT [dbo].[Feedbacks] ([UserId], [ProductId], [Comments], [Votes], [OrderId], [CreatedDate], [IsActive], [UpdatedDate], [AdminReply], [ReplyDate]) VALUES (19, 13, N'tốt ', 5, 1046, CAST(N'2023-11-13T15:42:21.8404173+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-13T15:42:21.8404174+00:00' AS DateTimeOffset), N'Cám ơn quý khách đã ủng hộ. Chúc quý khách một ngày tốt lành.', CAST(N'2023-12-02T04:56:23.1286174+00:00' AS DateTimeOffset))
INSERT [dbo].[Feedbacks] ([UserId], [ProductId], [Comments], [Votes], [OrderId], [CreatedDate], [IsActive], [UpdatedDate], [AdminReply], [ReplyDate]) VALUES (19, 14, NULL, 1, 1046, CAST(N'2023-11-13T15:42:21.8404184+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-13T15:42:21.8404184+00:00' AS DateTimeOffset), NULL, NULL)
INSERT [dbo].[Feedbacks] ([UserId], [ProductId], [Comments], [Votes], [OrderId], [CreatedDate], [IsActive], [UpdatedDate], [AdminReply], [ReplyDate]) VALUES (19, 15, NULL, 1, 1046, CAST(N'2023-11-13T15:42:21.8404186+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-13T15:42:21.8404186+00:00' AS DateTimeOffset), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (31, 14, N'Success', 14, N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 990000, N'A', CAST(N'2023-08-07T15:19:45.9493619+00:00' AS DateTimeOffset), 1, NULL, 0, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (32, 17, N'Cancel', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 1106500, N'A', CAST(N'2023-08-10T10:04:03.0710132+00:00' AS DateTimeOffset), 1, NULL, 0, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (38, 17, N'Cancel', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 1164500, N'A', CAST(N'2023-08-10T17:02:43.3673633+00:00' AS DateTimeOffset), 1, NULL, 0, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (39, 14, N'Pending', 14, N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 8000, N'A', CAST(N'2023-08-10T17:07:54.4729639+00:00' AS DateTimeOffset), 1, NULL, 0, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (40, 14, N'Processing', 14, N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 524250, N'A', CAST(N'2023-08-10T17:11:55.4854018+00:00' AS DateTimeOffset), 1, CAST(N'2023-12-02T02:38:41.6709580+00:00' AS DateTimeOffset), 0, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (41, 14, N'Pending', 14, N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 524250, N'A', CAST(N'2023-08-10T17:17:42.3342534+00:00' AS DateTimeOffset), 1, NULL, 0, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (42, 17, N'Cancel', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 240060, N'A', CAST(N'2023-08-10T17:22:46.4920694+00:00' AS DateTimeOffset), 1, NULL, 0, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (43, 17, N'Success', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 524250, N'A', CAST(N'2023-08-10T17:23:17.4424117+00:00' AS DateTimeOffset), 1, NULL, 0, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (44, 17, N'Success', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 574250, N'A', CAST(N'2023-08-10T17:32:17.0829635+00:00' AS DateTimeOffset), 1, NULL, 0, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (46, 14, N'Cancel', 14, N'tổ 8, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 3145500, N'A', CAST(N'2023-09-23T12:42:18.1090671+00:00' AS DateTimeOffset), 1, NULL, 0, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (1046, 19, N'Success', 19, N'279 Bưng Ông Thoàn, Phường Tăng Nhơn Phú B, Thành phố Thủ Đức, Thành phố Hồ Chí Minh', 322060, N'A', CAST(N'2023-10-22T06:10:19.4664733+00:00' AS DateTimeOffset), 1, NULL, 5, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (1047, 17, N'Success', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 1048500, N'A', CAST(N'2023-11-07T17:13:16.0830047+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-18T14:56:10.5869844+00:00' AS DateTimeOffset), 0, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (1048, 17, N'Success', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 520837.5, N'A', CAST(N'2023-11-18T14:58:35.5562998+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-18T14:59:33.2994576+00:00' AS DateTimeOffset), 5, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (1049, 17, N'Success', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 498037.5, N'A', CAST(N'2023-11-18T15:10:45.8202965+00:00' AS DateTimeOffset), 1, CAST(N'2023-11-18T15:12:41.4338489+00:00' AS DateTimeOffset), 5, N'', N'')
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (1050, 29, N'Pending', 29, N'97 Man Thiện, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 1244257.2, N'A', CAST(N'2023-12-03T05:38:23.6301909+00:00' AS DateTimeOffset), 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (1051, 17, N'Cancel', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 7360, N'A', CAST(N'2023-12-03T05:59:30.8174228+00:00' AS DateTimeOffset), 1, CAST(N'2023-12-03T05:59:58.3874929+00:00' AS DateTimeOffset), 8, NULL, NULL)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (1052, 17, N'Pending', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 7360, N'A', CAST(N'2023-12-03T06:01:50.0858865+00:00' AS DateTimeOffset), 1, NULL, 8, NULL, NULL)
INSERT [dbo].[Orders] ([Id], [UserId], [Status], [UpdatedBy], [Address], [TotalBill], [Payment], [CreatedDate], [IsActive], [UpdatedDate], [DiscountVIP], [Description], [Remark]) VALUES (1053, 17, N'Pending', 17, N'Ấp Bàu Chiên, Xã Tân Lâm, Huyện Xuyên Mộc, Tỉnh Bà Rịa - Vũng Tàu', 7360, N'A', CAST(N'2023-12-03T06:39:53.7606655+00:00' AS DateTimeOffset), 1, NULL, 8, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Orders] OFF
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
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1092, 11, 10000, 1048, 1, 20)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1093, 12, 699000, 1048, 1, 25)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1094, 14, 20000, 1048, 1, 20)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1095, 12, 699000, 1049, 1, 25)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1096, 12, 699000, 1050, 1, 25)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1097, 13, 100001, 1050, 8, 10)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1098, 11, 10000, 1051, 1, 20)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1099, 11, 10000, 1052, 1, 20)
INSERT [dbo].[OrderDetails] ([Id], [ProductId], [Price], [OrderId], [Quantity], [Discount]) VALUES (1100, 11, 10000, 1053, 1, 20)
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
