CREATE TABLE Tokens (
	Token			UNIQUEIDENTIFIER					NOT NULL,
	SpeakerId		SMALLINT							NOT NULL,
	Expires			DATETIME							NOT NULL,

	CONSTRAINT Tokens_PK PRIMARY KEY ( Token ),
	CONSTRAINT Tokens_Speakers_FK FOREIGN KEY ( SpeakerId ) REFERENCES Speakers( Id )
)