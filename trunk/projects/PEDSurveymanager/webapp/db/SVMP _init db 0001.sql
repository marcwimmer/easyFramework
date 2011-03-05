/*


	Autor: Marc Wimmer
	Date:  2004/06/29

	Initialization for pearson-specific adaptions of the surveymanager
*/
ALTER TABLE tdSurveys
ADD svy_coupon BIT NULL
GO
ALTER TABLE tdSurveys
ADD svy_coupon_price MONEY, svy_coupon_product_series NVARCHAR(64)
GO
ALTER TABLE tdSurveys
ADD svy_coupon_dbfield_email NVARCHAR(255),
svy_coupon_dbfield_speakto NVARCHAR(25),
svy_coupon_dbfield_firstname NVARCHAR(64), 
svy_coupon_dbfield_name NVARCHAR(64), 
svy_coupon_email_sender NVARCHAR(64)

GO

ALTER TABLE tdSurveys
ADD svy_coupon_source NVARCHAR(64),
svy_coupon_bprintable BIT, 
svy_coupon_downloadURL NVARCHAR(64)

GO

