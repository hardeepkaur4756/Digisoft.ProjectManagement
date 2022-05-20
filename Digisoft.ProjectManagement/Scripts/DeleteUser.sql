
Declare @UserId NVARCHAR(128)=''

if(@UserId is not null and @UserId!='' and @UserId!='62ab2752-d0c8-4be7-ba16-59806c303cc2')
BEGIN	
	Delete From ErrorLog Where CreatedBy=@UserId
    Delete FROM UserEducation where UserId=@UserId
    Delete From UserIncrement WHERE UserId = @UserId
    Delete From UserDocument WHERE UserId = @UserId
    Delete from UserDetail Where UserId= @UserId
    Delete from Working Where CreatedBy= @UserId
    Delete from Project Where CreatedBy= @UserId
    Delete from Client Where CreatedBy= @UserId
    Delete from AspNetUserRoles Where UserId= @UserId
	Delete from ASPNetUsers where Id = @UserId

END
ELSE
BEGIN
	SELECT 'Please Enter User Id' as Msg
END


