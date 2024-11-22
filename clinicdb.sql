use master
create database clinicdb
use clinicdb

create table [user]
(
	ID int IDENTITY(1,1),
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
	PRIMARY KEY(ID),
	UNIQUE(Email),
);

CREATE TABLE Patient
(
	ID int IDENTITY(1,1),
	SSNValidation BIT,
	PenaltyFees int,
	PRIMARY KEY (ID),
	FOREIGN KEY (ID) references [user],

);

CREATE TABLE Doctor 
(
	ID int IDENTITY(1,1),
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
	Foreign key (DoctorID) references Doctor,
);

CREATE TABLE DoctorExperience
(
	ExpID int IDENTITY(1,1),
	DoctorID int,
	Institution VARCHAR(40),
	Proof VARBINARY(max),
	SpanYears int CHECK (SpanYears <= 80 and SpanYears >=0),
	SpanMonths int CHECK (SpanMonths <= 12 and SpanMonths > 0),
	JobPosition VARCHAR(40),
	primary key(ExpID, DoctorID),
	foreign key (DoctorID) references Doctor,
)

CREATE TABLE DoctorCertificate
(
	CertID int IDENTITY(1,1),
	DoctorID int,
	date_of_acq date,
	Institute Varchar(40),
	Description VarChar(80),
	PRIMARY KEY (CertID, DoctorID),
	foreign key (DoctorID) references Doctor
);

CREATE TABLE FieldOfMedicine
(
	FieldCode int IDENTITY(1,1),
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
	SymptomID int IDENTITY(1,1),
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
	FakeID int IDENTITY(1,1),
	[type] VARCHAR(50),
	Severity int,
	DateOfFirstInstance Date,
	primary key(PatientID, FakeID),
	foreign key (PatientID) references Patient
);

CREATE TABLE Appointment
(
	AppointmentID int IDENTITY(1,1),
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

Create table Reviews
(
	DoctorID int,
	PatientID int,
	NStars int CHECK (NStars <=5 and NStars >=0),
	Comment VARCHAR(100),
	Foreign key (DoctorID) references Doctor,
	Foreign key (PatientID) references Patient,
);

Alter table [user]
ADD CONSTRAINT chk_valid_City_Governorate_user CHECK (
        (Governorate = 'Cairo' AND City IN ('Cairo', 'Nasr City', 'Heliopolis', 'Maadi', 'Shobra', 'Zamalek')) OR
        (Governorate = 'Giza' AND City IN ('Giza', '6th of October', 'El Sheikh Zayed', 'Haram', 'Dokki')) OR
        (Governorate = 'Alexandria' AND City IN ('Alexandria', 'Borg El Arab')) OR
        (Governorate = 'Aswan' AND City IN ('Aswan', 'Edfu', 'Kom Ombo')) OR
        (Governorate = 'Asyut' AND City IN ('Asyut', 'Dayrout', 'Manfalut')) OR
        (Governorate = 'Beheira' AND City IN ('Damanhour', 'Kafr El Dawwar', 'Edku')) OR
        (Governorate = 'Beni Suef' AND City IN ('Beni Suef', 'Al Fashn', 'Ihnasia')) OR
        (Governorate = 'Dakahlia' AND City IN ('Mansoura', 'Talkha', 'Mit Ghamr')) OR
        (Governorate = 'Damietta' AND City IN ('Damietta', 'Ras El Bar', 'Faraskur')) OR
        (Governorate = 'Faiyum' AND City IN ('Faiyum', 'Senoures', 'Itsa')) OR
        (Governorate = 'Gharbia' AND City IN ('Tanta', 'El Mahalla El Kubra', 'Kafr El Zayat')) OR
        (Governorate = 'Ismailia' AND City IN ('Ismailia', 'Fayed', 'Qantara')) OR
        (Governorate = 'Kafr El Sheikh' AND City IN ('Kafr El Sheikh', 'Baltim', 'Desouk')) OR
        (Governorate = 'Luxor' AND City IN ('Luxor', 'Armant', 'Esna')) OR
        (Governorate = 'Matrouh' AND City IN ('Marsa Matrouh', 'Sidi Abdel Rahman', 'El Alamein')) OR
        (Governorate = 'Minya' AND City IN ('Minya', 'Beni Mazar', 'Mallawi')) OR
        (Governorate = 'Monufia' AND City IN ('Shibin El Kom', 'Menouf', 'Quesna')) OR
        (Governorate = 'New Valley' AND City IN ('Kharga', 'Dakhla')) OR
        (Governorate = 'North Sinai' AND City IN ('Arish', 'Sheikh Zuwayed', 'Rafah')) OR
        (Governorate = 'Port Said' AND City IN ('Port Said', 'Port Fouad')) OR
        (Governorate = 'Qalyubia' AND City IN ('Benha', 'Shubra El Kheima', 'Kafr Shukr')) OR
        (Governorate = 'Qena' AND City IN ('Qena', 'Nag Hammadi', 'Qus')) OR
        (Governorate = 'Red Sea' AND City IN ('Hurghada', 'Safaga', 'Marsa Alam')) OR
        (Governorate = 'Sharqia' AND City IN ('Zagazig', 'Belbeis', '10th of Ramadan')) OR
        (Governorate = 'Sohag' AND City IN ('Sohag', 'Akhmim', 'Girga')) OR
        (Governorate = 'South Sinai' AND City IN ('Sharm El Sheikh', 'Dahab', 'El Tor')) OR
        (Governorate = 'Suez' AND City IN ('Suez', 'Ain Sokhna'))
)

Alter table DoctorCurWorkplace
ADD CONSTRAINT chk_valid_City_Governorate_doctorworkplace CHECK (
        (Governorate = 'Cairo' AND City IN ('Cairo', 'Nasr City', 'Heliopolis', 'Maadi', 'Shobra', 'Zamalek')) OR
        (Governorate = 'Giza' AND City IN ('Giza', '6th of October', 'El Sheikh Zayed', 'Haram', 'Dokki')) OR
        (Governorate = 'Alexandria' AND City IN ('Alexandria', 'Borg El Arab')) OR
        (Governorate = 'Aswan' AND City IN ('Aswan', 'Edfu', 'Kom Ombo')) OR
        (Governorate = 'Asyut' AND City IN ('Asyut', 'Dayrout', 'Manfalut')) OR
        (Governorate = 'Beheira' AND City IN ('Damanhour', 'Kafr El Dawwar', 'Edku')) OR
        (Governorate = 'Beni Suef' AND City IN ('Beni Suef', 'Al Fashn', 'Ihnasia')) OR
        (Governorate = 'Dakahlia' AND City IN ('Mansoura', 'Talkha', 'Mit Ghamr')) OR
        (Governorate = 'Damietta' AND City IN ('Damietta', 'Ras El Bar', 'Faraskur')) OR
        (Governorate = 'Faiyum' AND City IN ('Faiyum', 'Senoures', 'Itsa')) OR
        (Governorate = 'Gharbia' AND City IN ('Tanta', 'El Mahalla El Kubra', 'Kafr El Zayat')) OR
        (Governorate = 'Ismailia' AND City IN ('Ismailia', 'Fayed', 'Qantara')) OR
        (Governorate = 'Kafr El Sheikh' AND City IN ('Kafr El Sheikh', 'Baltim', 'Desouk')) OR
        (Governorate = 'Luxor' AND City IN ('Luxor', 'Armant', 'Esna')) OR
        (Governorate = 'Matrouh' AND City IN ('Marsa Matrouh', 'Sidi Abdel Rahman', 'El Alamein')) OR
        (Governorate = 'Minya' AND City IN ('Minya', 'Beni Mazar', 'Mallawi')) OR
        (Governorate = 'Monufia' AND City IN ('Shibin El Kom', 'Menouf', 'Quesna')) OR
        (Governorate = 'New Valley' AND City IN ('Kharga', 'Dakhla')) OR
        (Governorate = 'North Sinai' AND City IN ('Arish', 'Sheikh Zuwayed', 'Rafah')) OR
        (Governorate = 'Port Said' AND City IN ('Port Said', 'Port Fouad')) OR
        (Governorate = 'Qalyubia' AND City IN ('Benha', 'Shubra El Kheima', 'Kafr Shukr')) OR
        (Governorate = 'Qena' AND City IN ('Qena', 'Nag Hammadi', 'Qus')) OR
        (Governorate = 'Red Sea' AND City IN ('Hurghada', 'Safaga', 'Marsa Alam')) OR
        (Governorate = 'Sharqia' AND City IN ('Zagazig', 'Belbeis', '10th of Ramadan')) OR
        (Governorate = 'Sohag' AND City IN ('Sohag', 'Akhmim', 'Girga')) OR
        (Governorate = 'South Sinai' AND City IN ('Sharm El Sheikh', 'Dahab', 'El Tor')) OR
        (Governorate = 'Suez' AND City IN ('Suez', 'Ain Sokhna'))
);

ALTER TABLE Symptom
ADD CONSTRAINT chk_valid_symptom_type CHECK (
    [type] IN (
        'Fever', 'Chills', 'Sweating', 'Fatigue', 'Weakness', 'Weight loss', 'Weight gain',
        'Loss of appetite', 'Dehydration', 'Swelling', 'Headache', 'Migraine', 'Back pain',
        'Neck pain', 'Chest pain', 'Abdominal pain', 'Pelvic pain', 'Joint pain', 'Muscle pain',
        'Cough', 'Shortness of breath', 'Wheezing', 'Sore throat', 'Hoarseness', 'Nasal congestion',
        'Runny nose', 'Sneezing', 'Nausea', 'Vomiting', 'Diarrhea', 'Constipation', 'Heartburn',
        'Bloating', 'Abdominal cramps', 'Blood in stool', 'Dizziness', 'Vertigo', 'Fainting',
        'Tremors', 'Numbness', 'Tingling', 'Seizures', 'Memory loss', 'Anxiety', 'Depression',
        'Insomnia', 'Mood swings', 'Irritability', 'Confusion', 'Hallucinations', 'Palpitations',
        'High blood pressure', 'Low blood pressure', 'Cold hands and feet', 'Rash', 'Itching',
        'Redness', 'Dry skin', 'Bruising', 'Blisters', 'Peeling skin', 'Blurred vision', 'Double vision',
        'Eye redness', 'Eye pain', 'Watery eyes', 'Light sensitivity', 'Hearing loss', 'Tinnitus',
        'Ear pain', 'Ear discharge', 'Frequent urination', 'Painful urination', 'Blood in urine',
        'Incontinence', 'Urgency to urinate', 'Irregular periods', 'Heavy bleeding', 'Pelvic pain',
        'Discharge', 'Erectile dysfunction', 'Hair loss', 'Cold intolerance', 'Heat intolerance',
        'Enlarged lymph nodes', 'Difficulty swallowing', 'Hiccups'
    )
);

ALTER TABLE LongTermConditions
ADD CONSTRAINT chk_valid_condition_type CHECK (
    type IN (
        'Hypertension', 'Coronary Artery Disease', 'Heart Failure', 'Arrhythmia', 'Stroke',
        'Peripheral Artery Disease', 'Asthma', 'Chronic Obstructive Pulmonary Disease (COPD)',
        'Sleep Apnea', 'Pulmonary Fibrosis', 'Alzheimer’s Disease', 'Parkinson’s Disease',
        'Multiple Sclerosis', 'Epilepsy', 'Depression', 'Bipolar Disorder', 'Schizophrenia',
        'Anxiety Disorders', 'Diabetes Type 1', 'Diabetes Type 2', 'Hypothyroidism',
        'Hyperthyroidism', 'Polycystic Ovary Syndrome (PCOS)', 'Cushing''s Syndrome',
        'Inflammatory Bowel Disease (IBD)', 'Crohn’s Disease', 'Ulcerative Colitis',
        'Irritable Bowel Syndrome (IBS)', 'Celiac Disease', 'Gastroesophageal Reflux Disease (GERD)',
        'Osteoarthritis', 'Rheumatoid Arthritis', 'Osteoporosis', 'Gout', 'Ankylosing Spondylitis',
        'Psoriasis', 'Eczema', 'Vitiligo', 'Lupus', 'Breast Cancer', 'Lung Cancer', 'Prostate Cancer',
        'Colorectal Cancer', 'Leukemia', 'Lymphoma', 'Systemic Lupus Erythematosus (SLE)',
        'Sjögren’s Syndrome', 'Hashimoto’s Thyroiditis', 'Chronic Kidney Disease (CKD)',
        'Polycystic Kidney Disease', 'Kidney Stones', 'HIV/AIDS', 'Chronic Fatigue Syndrome',
        'Fibromyalgia', 'Sickle Cell Disease', 'Hemophilia', 'Thalassemia'
    )
);


ALTER TABLE FieldOfMedicine
ADD CONSTRAINT chk_valid_field_name CHECK (
    FieldName IN (
        'General Medicine', 'Family Medicine', 'Internal Medicine', 'General Surgery',
        'Cardiothoracic Surgery', 'Neurosurgery', 'Orthopedic Surgery', 'Plastic Surgery',
        'Pediatric Surgery', 'Vascular Surgery', 'Urological Surgery', 'Cardiology',
        'Endocrinology', 'Gastroenterology', 'Hematology', 'Nephrology', 'Oncology',
        'Pulmonology', 'Rheumatology', 'Infectious Diseases', 'General Pediatrics',
        'Neonatology', 'Pediatric Cardiology', 'Pediatric Neurology', 'Obstetrics',
        'Gynecology', 'Maternal-Fetal Medicine', 'Reproductive Endocrinology',
        'Emergency Medicine', 'Critical Care Medicine', 'Trauma Medicine',
        'Anesthesiology', 'Pain Medicine', 'Neurology', 'Psychiatry',
        'Child and Adolescent Psychiatry', 'Diagnostic Radiology', 'Interventional Radiology',
        'Nuclear Medicine', 'Anatomical Pathology', 'Clinical Pathology',
        'Forensic Pathology', 'General Dermatology', 'Cosmetic Dermatology',
        'Ophthalmology', 'Otolaryngology (ENT)', 'Public Health', 'Occupational Medicine',
        'Preventive Medicine', 'Geriatrics', 'Sports Medicine', 'Addiction Medicine',
        'Palliative Care', 'Medical Genetics'
    )
);
