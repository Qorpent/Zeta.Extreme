IF SCHEMA_ID('zetaes') IS NULL EXEC sp_executesql N'create schema zetaes'
GO

IF OBJECT_ID('zetaes.user_exists') IS NOT NULL DROP PROC zetaes.user_exists
GO
create proc zetaes.user_exists @login nvarchar(255), @doselect bit = 0 as begin
	declare @result int;
	set @result = ISNULL((select id from zetai.[user] where code = @login or advlogins like '%/'+@login+'/%'),0)
	if @doselect=1 select @result
	return @result
end;
GO
IF OBJECT_ID('zetaes.user_create') IS NOT NULL DROP PROC zetaes.user_create
GO
create proc zetaes.user_create @login nvarchar(255), @doselect bit = 0 as begin
	declare @result int;
	exec @result = zetaes.user_exists @login
	if @result=0 begin
		insert zetai.[user] ( code ) values (@login);
		set @result = (select id from zetai.[user] where code = @login);
	end;
	if @doselect=1 select @result;
	return @result;
end;
GO


IF OBJECT_ID('zetaes.user_set_system_role') IS NOT NULL DROP PROC zetaes.user_set_system_role
GO
create proc zetaes.user_set_system_role @login nvarchar(255), @role nvarchar(255), @doselect bit = 0  as begin
	declare @id int, @currentroles nvarchar(255)
	exec @id = zetaes.user_create @login
	set @currentroles = ISNULL((select sysroles from zetai.[user] where id = @id),'')
	if not @currentroles like '%/'+@role+'/%' begin
		update zetai.[user] set sysroles = replace(@currentroles+'/'+@role+'/','//','/');
		if @doselect=1 select 1;
		return 1;
	end;
	if @doselect=1 select 0;
	return 0;
end;
GO

IF OBJECT_ID('zetaes.user_del_system_role') IS NOT NULL DROP PROC zetaes.user_del_system_role
GO
create proc zetaes.user_del_system_role @login nvarchar(255), @role nvarchar(255) , @doselect bit = 0 as begin
	declare @id int, @currentroles nvarchar(255), @newroles nvarchar(255)
	exec @id = zetaes.user_exists @login
	if 0 = @id begin
		if @doselect=1 select 0;
		return 0;
	end
	set @currentroles = ISNULL((select sysroles from zetai.[user] where id = @id),'')
	if  @currentroles like '%/'+@role+'/%' begin
		set @newroles = replace(@currentroles,'/'+@role+'/','/')
		if @newroles = '/' set @newroles = '';
		update zetai.[user] set sysroles = @newroles where id =@id
		if @doselect=1 select 1;
		return 1;
	end;
	if @doselect=1 select 1;
	return 0;
end;
GO


delete zetai.[usercard] 

delete zetai.[user] 

dbcc checkident ('zetai.[user]', RESEED, 0)
exec zetaes.user_create 'ugmk\sfo'
update zetai.[user] set advlogins = '/local\comdiv/' where id = 1
exec zetaes.user_exists 'ugmk\sfo'
exec zetaes.user_exists 'local\comdiv'
exec  zetaes.user_set_system_role 'local\comdiv','ADMIN'
exec  zetaes.user_set_system_role 'local\comdiv','DEVELOPER'
exec  zetaes.user_set_system_role 'local\comdiv','FIXER'
select * from zetai.[user]
exec  zetaes.user_del_system_role 'local\comdiv','FIXER'
select * from zetai.[user]
exec  zetaes.user_del_system_role 'local\comdiv','DEVELOPER'
select * from zetai.[user]
exec  zetaes.user_del_system_role 'local\comdiv','ADMIN'
select * from zetai.[user]