CREATE TABLE [dbo].[BoardNotices]
(
	[Id] INT NOT NULL PRIMARY KEY identity (1,1),		-- Serial Number 
	[ParentId] Int Null,		-- ParentId, AppId, SiteId, ... 
	[ParentKey] nvarchar(255) null,					-- 부모의 GUID 
	[Name] NVarchar(255) Not Null,					-- 작성자 
	[Title] NVarchar(255) Null,						-- 제목
	[Content] nvarchar(max) null,					-- 내용 => Todo: not null 
	[IsPinned] Bit null default(0),					-- 공지글로 올리기 
	[CreatedBy]	nvarchar(255) null,					-- 등록자(creator)
	[Created] datetime default(getdate()) null,		-- 생성일 
	[ModifiedBy] nvarchar(255) null,				-- 수정자
	[Modified] datetime null,						-- 수정일 

	-- [2] 자료실 게시판 관련 주요 컬럼
	FileName	nvarchar(255)	null,	-- 파일명
	FileSize	Int	Default 0,			-- 파일크기 
	DownCount	Int	Default 0,			-- 다운수 
)
Go 
