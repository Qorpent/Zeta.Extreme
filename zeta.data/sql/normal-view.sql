/****** Object:  View [zeta].[normalusr]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalusr]
GO
/****** Object:  View [zeta].[normalrow]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalrow]
GO
/****** Object:  View [zeta].[normalperiod]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalperiod]
GO
/****** Object:  View [zeta].[normalobjtype]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalobjtype]
GO
/****** Object:  View [zeta].[normalobjclass]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalobjclass]
GO
/****** Object:  View [zeta].[normalobj]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalobj]
GO
/****** Object:  View [zeta].[normallink]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normallink]
GO
/****** Object:  View [zeta].[normalhist]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalhist]
GO
/****** Object:  View [zeta].[normalformstate]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalformstate]
GO
/****** Object:  View [zeta].[normalform]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalform]
GO
/****** Object:  View [zeta].[normaldiv]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normaldiv]
GO
/****** Object:  View [zeta].[normalcol]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalcol]
GO
/****** Object:  View [zeta].[normalcell]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalcell]
GO
/****** Object:  View [zeta].[normalbizprocess]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalbizprocess]
GO
/****** Object:  View [zeta].[normalbizprocess]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalzone]
GO
/****** Object:  View [zeta].[normalbizprocess]    Script Date: 21.09.2013 23:35:28 ******/
DROP VIEW [zeta].[normalregion]
GO
DROP VIEW [zeta].[normalpoint]
GO

DROP VIEW [zeta].[normalobjrole]
GO

CREATE view [zeta].[normalzone] as
select Id,Code,Name,isnull(Comment,'') as Comment,isnull(Tag,'') as Tag,Version
from zetai.Zone
go
CREATE view [zeta].[normalregion] as
select Id,Code,Name,isnull(Comment,'') as Comment,isnull(Tag,'') as Tag,Version,Zone as ZoneId
from zetai.Region
go
CREATE view [zeta].[normalpoint] as
select Id,Code,Name,isnull(Comment,'') as Comment,isnull(Tag,'') as Tag,Version,Region as RegionId
from zetai.Point
go
CREATE view [zeta].[normalobjrole] as
select Id,Code,Name,isnull(Comment,'') as Comment,isnull(Tag,'') as Tag,Version
from zetai.objrole
go



/****** Object:  View [zeta].[normalbizprocess]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [zeta].[normalbizprocess] as
select Id,Code,Name,Comment,Tag,
isnull(InProcess,'')as InProcess,
isnull(Role,'') as Role, 
isnull(IsFinal,0) as IsFinal,
isnull(RootRows,'') as RootRows,
isnull(Process,'') as Process,
Version
from zetai.BizProcess


GO
/****** Object:  View [zeta].[normalcell]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [zeta].[normalcell] as
select Id,Version,Year,Kvart as Period,row as RowId, col as ColId, Obj as ObjId, detail as DetailId,
isnull(DecimalValue,0) as DecimalValue,isnull(StringValue,'')as StringValue,isnull(Usr,'sys') as Usr,isnull(Valuta,'NONE') as Currency,altobj as ContragentId
from cell

GO
/****** Object:  View [zeta].[normalcol]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [zeta].[normalcol] as
select 
Id, 
Version, 
DataType, 
IsFormula, 
isnull(Formula,'') as Formula, 
isnull(FormulaEvaluator,'') as FormulaType, 
Name, 
Code, 
isnull(Comment,'') as Comment, 
--Uid, 
isnull(Measure,'') as Measure, 
--IsDynamicMeasure, 
--DataTypeDetail, 
--LookUp, 
--IsDinamycLookUp, 
--ParsedFormula, 
--Pkg, 
isnull(valuta,'') as Valuta, 
isnull(tag,'') as Tag, 
isnull(markcache,'') as MarkCache, 
idx as Idx, 
isnull(usr,'') as Usr
--formulatype
from [usm].[ValueType]

GO
/****** Object:  View [zeta].[normaldiv]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [zeta].[normaldiv] as select Id,Code,Name,isnull(Comment,'') as Comment,Version,isnull(Tag,'') as Tag,Idx from usm.Holding

GO
/****** Object:  View [zeta].[normalform]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [zeta].[normalform] as 
select Id, Version, Code, Year, Period, isnull(Template,'') as LockCode, Object
--,isnull( srcid,0) as SrcId, isnull(srccode,'') as SrcCode,  idx as Idx, usr as Usr, tag as Tag, isnull( templatecode,'') as TemplateCode
from usm.form

GO
/****** Object:  View [zeta].[normalformstate]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [zeta].[normalformstate] as 
select s.Id, s.Version, s.Comment, s.State, s.Usr, s.Form, s.Parent, isnull(s.idx,0) as Idx, isnull( s.tag ,'') as Tag,
f.Year as Year, f.Period as Period, isnull(f.Template,'') as LockCode, f.Object
from usm.formstate s join usm.form f on s.form = f.id

GO
/****** Object:  View [zeta].[normalhist]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [zeta].[normalhist] as select id as Id,Ver as Time,rowid as CellId, bizkey as BizKey, dval as Value, wasd as Deleted,
 isnull(usr,'sys') as Usr from usm.cellversion

GO
/****** Object:  View [zeta].[normallink]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [zeta].[normallink] as
select 
id, code, srctype, trgtype, src, trg, type, subtype, tag, active, start, finish, usr, value, sysfield, isauto
from comdiv.metalink

GO
/****** Object:  View [zeta].[normalobj]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [zeta].[normalobj] as
select
	Id,Code,Name,isnull(Comment,'') as Comment, Version, 	isnull(ShortName,'') as ShortName, isnull(OuterCode,'') as OuterCode,  	-- 0 - 6
	Municipal as ZoneId,	-- 7
	Otrasl as ObjRoleId,    -- 8
	Holding as ObjDivId,	-- 9
	isnull(showOnStartPage,0) as Main, --10
	parent as ParentId,		path as Path, -- 11 - 12
	type as TypeId, --13
	isnull(IsFormula,0) as IsFormula, isnull(formula,'') as Formula, isnull(FormulaType,'') as FormulaType,	-- 14 -16
	isnull(tag,'') as Tag, isnull(GroupCache,'') as GroupCache, --17 -18
	isnull(valuta,'') as Valuta,					--19			
	isnull(Role,'') as [Role],Active, isnull(Start,'1900-01-01') as Start, isnull(Finish,'3000-01-01') as Finish ,isnull(isinner,0) as IsInner --20 -24
from usm.Org

GO
/****** Object:  View [zeta].[normalobjclass]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [zeta].[normalobjclass] as
select
	Id,Code,Name,isnull(Comment,'') as Comment, Version,  isnull(Tag,'') as Tag
from usm.DetailObjectClass

GO
/****** Object:  View [zeta].[normalobjtype]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [zeta].[normalobjtype] as
select
	Id,Code,Name,isnull(Comment,'') as Comment, Version, Class as ClassId, isnull(Tag,'') as Tag
from usm.DetailObjectType

GO
/****** Object:  View [zeta].[normalperiod]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE view [zeta].[normalperiod] as select ClassicId as Id, Name, isnull(IsFormula,0) as IsFormula, isnull(Formula,'') as Formula,
isnull(Tag,'') as Tag,isnull(Idx,0) as Idx,isnull(MonthCount,0) as MonthCount, isnull(StartDate,'1900-01-01') as Start, 
isnull(EndDate,'3000-01-01') as Finish,isnull(Category,'nocategory') as Category, Version from usm.Period



GO
/****** Object:  View [zeta].[normalrow]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [zeta].[normalrow] as 
select 
Id, 
Version, 
IsFormula, 
isnull(Formula,'') as Formula, 
isnull(FormulaEvaluator,'') as FormulaType, 
--Data, 
Name, 
Code, 
isnull(Comment,'')as Comment, 
--Uid, 
MainDataTreeRefTo as RefId, 
MainDataTree as ParentId, 
Org as ObjectId, 
isnull(OuterCode,'') as OuterCode, 
Idx, 
isnull(Measure,'')as Measure, 
--LookUp, 
Path, 
isnull(Grp,'') as Grp, 
isnull(MarkCache,'') as MarkCache, 
--ParsedFormula, 
--IsDynamicMeasure, 
--IsDinamycLookUp, 
--Pkg, 
--OnbjectGroups, 
--ObjectGroups, 
--FormElementType, 
--Validator, 
--NoAuto, 
--ColumnSubstitution, 
--FullName, 
--Role, 
--sourcerow, 
isnull(valuta,'') as Valuta, 
isnull(tag,'') as Tag, 
isnull(usr,'') as Usr, 
--parent, oldparent, 
exrefto as ExRefId ,
isnull(ExtremeFormulaMode,0) as ExtremeFormulaMode
from usm.maindatatree
--ref, active, SchemaVersion, lc, lc1, lc2

GO
/****** Object:  View [zeta].[normalusr]    Script Date: 21.09.2013 23:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [zeta].[normalusr] as select
	Id, Version, Boss as ObjAdmin, Dolzh, isnull(Contact,'') as Contact, 
	Name,isnull(Comment,'') as Email, Uid, Org as ObjId,  (select name from usm.org where id = Org) as ObjName,
	isnull(Login,'') as Login, Active, isnull(Roles,'')as Roles, SlotList
from usm.underwriter

GO
