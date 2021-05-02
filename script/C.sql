﻿/*---------------------------------------------------------- 
MASV: 18120397 - 18120399 - 18120423
HO TEN CAC THANH VIEN NHOM: 
	Nguyễn Đặng Hồng Huy
	Phạm Đức Huy
	Trịnh Tấn Khoa
LAB: 03 - NHOM 
NGAY: 28/04/2021
----------------------------------------------------------*/ 
USE [QLSVNhom]
GO

IF OBJECTPROPERTY(OBJECT_ID('dbo.SP_INS_PUBLIC_NHANVIEN'), N'IsProcedure') = 1 
    DROP PROCEDURE [dbo].[SP_INS_PUBLIC_NHANVIEN]
GO
CREATE PROCEDURE SP_INS_PUBLIC_NHANVIEN 
	@MANV	VARCHAR(20) ,
	@HOTEN	NVARCHAR(100),
	@EMAIL	VARCHAR (20),
	@LUONGCB	VARCHAR(100),
	@TENDN	NVARCHAR(100),
	@MK		VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @SQL NVARCHAR(MAX);
	IF ASYMKEY_ID(@MANV) IS NULL
    BEGIN
        SET @SQL = 'CREATE ASYMMETRIC KEY '  + QUOTENAME(@MANV) + ' ' +
                 'WITH ALGORITHM = RSA_2048 ' + 
                 'ENCRYPTION BY PASSWORD = ' + QUOTENAME(@MK , NCHAR(39))
        EXEC (@SQL)
    END

	DECLARE @MATKHAU_SHA1 VARBINARY(MAX);
	SET @MATKHAU_SHA1 = CONVERT(VARBINARY(MAX),HASHBYTES('SHA1', @MK));

	DECLARE @LUONG_RSA512 VARBINARY(MAX);
	SET @LUONG_RSA512 = ENCRYPTBYASYMKEY(ASYMKEY_ID(@MANV), @LUONGCB);

	DECLARE @PUBKEY NVARCHAR(20);
	SELECT @PUBKEY = CONVERT(NVARCHAR(20),@MANV);

	INSERT INTO DBO.NHANVIEN
	VALUES (@MANV, @HOTEN, @EMAIL, @LUONG_RSA512, @TENDN, @MATKHAU_SHA1,@PUBKEY);
END
GO

EXEC SP_INS_PUBLIC_NHANVIEN 'NV01', 'NGUYEN VAN A','nva@yahoo.com', 3000000, 'NVA', '123456'
GO 
EXEC SP_INS_PUBLIC_NHANVIEN 'NV02', 'NGUYEN VAN B','nvb@yahoo.com', 2000000, 'NVB', '1234567'
GO 

IF OBJECTPROPERTY(OBJECT_ID('dbo.SP_SEL_PUBLIC_NHANVIEN'), N'IsProcedure') = 1 
    DROP PROCEDURE [dbo].[SP_SEL_PUBLIC_NHANVIEN]
GO
CREATE PROCEDURE SP_SEL_PUBLIC_NHANVIEN 
	@TENDN	NVARCHAR(100),
	@MK		VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (SELECT * FROM DBO.NHANVIEN WHERE TENDN = @TENDN)
	BEGIN
		DECLARE @MANV VARCHAR(20);
		SET @MANV = (SELECT MANV FROM DBO.NHANVIEN WHERE TENDN = @TENDN);
		SELECT MANV, HOTEN, EMAIL, CONVERT(VARCHAR(MAX), DECRYPTBYASYMKEY(ASYMKEY_ID(@MANV),LUONG, CONVERT(NVARCHAR(100),@MK))) "LUONGCB"
		FROM NHANVIEN;
	END	
END
  
--SELECT * FROM DBO.NHANVIEN
--GO
--EXEC SP_SEL_PUBLIC_NHANVIEN 'NVA','123456'
--GO
--EXEC SP_SEL_PUBLIC_NHANVIEN 'NVB','1234567'
--GO

