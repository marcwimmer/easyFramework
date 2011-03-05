/*


	Autor: Marc Wimmer
	Date:  2004/06/29

	Initialization for pearson-specific adaptions of the surveymanager
*/
ALTER TABLE tdSurveys
ADD svy_newsletter_active BIT, svy_newsletter_list NVARCHAR(64)
GO


ALTER TABLE tdSurveys
DROP COLUMN svy_coupon_dbfield_email, svy_coupon_dbfield_speakto, svy_coupon_dbfield_firstname, svy_coupon_dbfield_name
GO