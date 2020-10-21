-----------------------------------------------------------------
-- Updates Table
-----------------------------------------------------------------

CREATE TABLE Updates (
	ScriptName			NVARCHAR(256)					NOT NULL
);

-----------------------------------------------------------------
-- Events Table
-----------------------------------------------------------------

CREATE TABLE Events (
	Id					INT								NOT NULL,
	[Name]				NVARCHAR(15)					NOT NULL,
	StartDate			SMALLDATETIME					NOT NULL,
	EndDate				SMALLDATETIME					NOT NULL,

	CONSTRAINT Events_PK PRIMARY KEY ( Id )
);

-----------------------------------------------------------------
-- SponsorLevels Table
-----------------------------------------------------------------

CREATE TABLE SponsorLevels (
	Id					INT				IDENTITY(1,1)	NOT NULL,
	DisplayOrder		INT								NOT NULL,
	[Name]				NVARCHAR(24)					NOT NULL,
	Cost				INT								NOT NULL,
	DisplayInEmails		BIT								NOT NULL,
	DisplayInSidebar	BIT								NOT NULL,
	DisplayLink			BIT								NOT NULL,
	TimeOnScreen		INT								NOT NULL,
	Tickets				INT								NOT NULL,
	Discount			INT								NOT NULL,
	PreConEmail			BIT								NOT NULL,
	MidConEmail			BIT								NOT NULL,
	PostConEmail		BIT								NOT NULL,

	CONSTRAINT SponsorLevels_PK PRIMARY KEY NONCLUSTERED ( Id ),
	CONSTRAINT SponsorLevels_CI	UNIQUE CLUSTERED ( DisplayOrder )
);

-----------------------------------------------------------------
-- Companies Table
-----------------------------------------------------------------

CREATE TABLE Companies (
	Id					INT				IDENTITY(1,1)	NOT NULL,
	[Name]				NVARCHAR(50)					NOT NULL,
	[Address]			NVARCHAR(200)					NOT NULL,
	Phone				VARCHAR(20)						NOT NULL,
	Website				VARCHAR(100)					NOT NULL,
	Twitter				NVARCHAR(20)					NULL,

	CONSTRAINT Companies_PK PRIMARY KEY ( Id )
);

GO

-----------------------------------------------------------------
-- Sponsors Table
-----------------------------------------------------------------

CREATE TABLE Sponsors (
	Id					INT				IDENTITY(1,1)	NOT NULL,
	EventId				INT								NOT NULL,
	CompanyId			INT								NOT NULL,
	SponsorLevelId		INT								NOT NULL,

	CONSTRAINT Sponsors_PK PRIMARY KEY ( Id ),
	CONSTRAINT Sponsors_Events_FK FOREIGN KEY ( EventId ) REFERENCES Events ( Id ) ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT Sponsors_Companies_FK FOREIGN KEY ( CompanyId ) REFERENCES Companies ( Id ) ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT Sponsors_SponsorLevels_FK FOREIGN KEY ( SponsorLevelId ) REFERENCES SponsorLevels ( Id ) ON UPDATE CASCADE ON DELETE CASCADE
);
