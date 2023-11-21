CREATE TABLE Projects (
	Id INT IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(MAX) NOT NULL,
    Description nvarchar(MAX),
	CreatedDate DATE NULL DEFAULT(GETDATE()),
	CreatedBy varchar(50) NULL,
	LastUpdatedBy varchar(50) NULL
)

CREATE TABLE Progressions (
	Id INT IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(MAX) NOT NULL
)


INSERT INTO Progressions (Name) SELECT 'Started' WHERE NOT EXISTS (SELECT 1 FROM Progressions WHERE Name = 'Started')
INSERT INTO Progressions (Name) SELECT 'In Progress' WHERE NOT EXISTS (SELECT 1 FROM Progressions WHERE Name = 'In Progress')
INSERT INTO Progressions (Name) SELECT 'Completed' WHERE NOT EXISTS (SELECT 1 FROM Progressions WHERE Name = 'Completed')

CREATE TABLE Phases (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name nvarchar(MAX) NOT NULL,
    Description nvarchar(MAX)
)

INSERT INTO Phases (Name) SELECT 'Planning' WHERE NOT EXISTS (SELECT 1 FROM Phases WHERE Name = 'Planning')
INSERT INTO Phases (Name) SELECT 'Feasibility and Analysis' WHERE NOT EXISTS (SELECT 1 FROM Phases WHERE Name = 'Feasibility and Analysis')
INSERT INTO Phases (Name) SELECT 'Design' WHERE NOT EXISTS (SELECT 1 FROM Phases WHERE Name = 'Design')
INSERT INTO Phases (Name) SELECT 'Software Development' WHERE NOT EXISTS (SELECT 1 FROM Phases WHERE Name = 'Software Development')
INSERT INTO Phases (Name) SELECT 'Implementation' WHERE NOT EXISTS (SELECT 1 FROM Phases WHERE Name = 'Implementation')
INSERT INTO Phases (Name) SELECT 'Operation and Maintenance' WHERE NOT EXISTS (SELECT 1 FROM Phases WHERE Name = 'Operation and Maintenance')
INSERT INTO Phases (Name) SELECT 'No Phase' WHERE NOT EXISTS (SELECT 1 FROM Phases WHERE Name = 'No Phase')


CREATE TABLE ProjectPhases (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	ProjectId INT NOT NULL,
	PhaseId INT NOT NULL,
	OrderInSequence INT NULL,
	FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    FOREIGN KEY (PhaseId) REFERENCES Phases(Id)
)

CREATE TABLE Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(MAX) NOT NULL,
    Description nvarchar(MAX) NULL,
	ProjectPhaseId INT,
	ProgressionId INT NULL,
	CreatedDate DATE NULL DEFAULT(GETDATE()),
    DueDate DATE NULL,
	CreatedBy varchar(50) NULL,
	LastUpdatedBy varchar(50) NULL,
    Priority INT NULL,
	FOREIGN KEY (ProjectPhaseId) REFERENCES ProjectPhases(Id),
	FOREIGN KEY (ProgressionId) REFERENCES Progressions(Id)
    -- Add more columns as needed
);

CREATE TABLE TaskDependencies (
    DependentTaskId INT,
    DependencyTaskId INT,
    PRIMARY KEY (DependentTaskId, DependencyTaskId),
    FOREIGN KEY (DependentTaskId) REFERENCES Tasks(Id) ON DELETE CASCADE,
    FOREIGN KEY (DependencyTaskId) REFERENCES Tasks(Id)
);





