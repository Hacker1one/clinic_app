
Create database clinic_draft
use clinic_draft

create table [user]
(
	ID int,
	FName VARCHAR(20) NOT NULL,
	LName VARCHAR(20) NOT NULL,
	SSN int,
	RegistrationDate Date NOT NULL,
	Gender CHAR,
	[Password] VARCHAR(40),
	BirthDate Date,
	City VARCHAR(30),
	Governorate VARCHAR(30),
	Email VARCHAR(50),
	PRIMARY KEY(id),
	UNIQUE(Email),
);

CREATE TABLE Patient
(
	ID int,
	SSNValidation BIT,
	PenaltyFees int,
	PRIMARY KEY (ID),
	FOREIGN KEY (ID) references [user],

);

CREATE TABLE Doctor 
(
	ID int,
	PricePA int,
	SSNValidation BIT,
	Banned BIT,
	SSN int,
	PRIMARY KEY (ID),
	FOREIGN KEY (ID) references [user],
);

CREATE TABLE DoctorCurWorkplace
(
	City VARCHAR(20),
	Governorate VARCHAR(20),
	Institution VARCHAR(40),
	JobPosition VARCHAR (30),
	DoctorID int,
	PRIMARY KEY(DoctorID, City, Governorate, Institution, JobPosition),
);

CREATE TABLE DoctorExperience
(
	ExpID int,
	DoctorID int,
	Institution VARCHAR(40),
	Proof VARBINARY(max),
	SpanYears int,
	SpanMonths int,
	JobPosition VARCHAR(40),
	primary key(ExpID, DoctorID),
	foreign key (DoctorID) references Doctor,
)

CREATE TABLE DoctorCertificate
(
	CertID int,
	DoctorID int,
	date_of_acq date,
	Institute Varchar(40),
	Description VarChar(80),
	PRIMARY KEY (CertID, DoctorID),
	foreign key (DoctorID) references Doctor
);

CREATE TABLE FieldOfMedicine
(
	FieldCode int,
	FieldName VARCHAR(30),
	FDescription VARCHAR(50),
	CommonConditions VARCHAR(50),
	PRIMARY KEY (FieldCode)
);

ALTER TABLE Doctor
ADD FieldCode int,
Constraint fk_fieldcode Foreign KEY (FieldCode) REFERENCES FieldOFMedicine

CREATE TABLE Symptom
(
	PatientID int,
	SymptomID int,
	[type] VARCHAR(50),
	DateOfFirstInstance Date,
	Severity int,
	OnsetDuration_years int,
	OnsetDuration_months int,
	OnsetDuration_days int,
	IsPresent BIT,
	primary key(PatientID, SymptomID),
	foreign key (PatientID) references Patient
);

Create Table LongTermConditions
(
	PatientID int,
	FakeID int,
	[type] VARCHAR(50),
	Severity int,
	DateOfFirstInstance Date,
	primary key(PatientID, FakeID),
	foreign key (PatientID) references Patient
);

CREATE TABLE Appointment
(
	AppointmentID int,
	DoctorID int,
	PatientID int,
	IsFinished BIT,
	IsConfirmed BIT,
	DatenTime DateTime,
	PRIMARY KEY (AppointmentID, DoctorID),
	FOREIGN KEY (PatientID) references Patient,
	Foreign key (DoctorID) references Doctor,
);

Create table Diagnosis
(
	AppointmentID int,
	DoctorID int,
	PatientID int,
	condition VARCHAR(50),
	[description] VARCHAR(100),
	PRIMARY KEY (AppointmentID, DoctorID, PatientID),
	FOREIGN KEY (AppointmentID, DoctorID) references Appointment,
	FOREIGN KEY (PatientID) references Patient
);

