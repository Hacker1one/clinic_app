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
	[Type] CHAR NOT NULL,
	PRIMARY KEY(ID),
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





INSERT INTO [user] (FName, LName, SSN, RegistrationDate, Gender, [Password], BirthDate, City, Governorate, Email, [Type])
VALUES 

('John', 'Doe', 123456789, '2023-01-01', 'M', 'password123', '1985-05-15', 'Cairo', 'Cairo', 'john.doe@example.com', 'p'),
('Jane', 'Smith', 987654321, '2023-01-02', 'F', 'securepass', '1990-07-20', 'Giza', 'Giza', 'jane.smith@example.com', 'p'),
('Ahmed', 'Ali', 223344556, '2023-01-03', 'M', 'ahmedpass', '1975-09-12', 'Alexandria', 'Alexandria', 'ahmed.ali@example.com', 'p'),
('Sara', 'Hassan', 445566778, '2023-01-04', 'F', 'sarapass', '1992-03-25', 'Aswan', 'Aswan', 'sara.hassan@example.com', 'p'),
('Mohamed', 'Youssef', 112233445, '2023-01-05', 'M', 'mypassword', '1987-09-18', 'Zamalek', 'Cairo', 'mohamed.youssef@example.com', 'p'),
('Nora', 'Farid', 998877665, '2023-01-06', 'F', 'norapass', '1993-11-25', 'Heliopolis', 'Cairo', 'nora.farid@example.com', 'p'),
('Ali', 'Kamal', 667788990, '2023-01-07', 'M', 'alikpass', '1988-03-19', 'Nasr City', 'Cairo', 'ali.kamal@example.com', 'p'),
('Mona', 'Gamal', 445566334, '2023-01-08', 'F', 'monag123', '1991-06-14', 'Maadi', 'Cairo', 'mona.gamal@example.com', 'p'),
('Hassan', 'Tariq', 332211009, '2023-01-09', 'M', 'hassanpass', '1979-12-11', 'Shobra', 'Cairo', 'hassan.tariq@example.com', 'p'),
('Layla', 'Nashat', 776655443, '2023-01-10', 'F', 'laylapass', '1995-02-28', 'Zamalek', 'Cairo', 'layla.nashat@example.com', 'p'),
('Tamer', 'Saad', 123123123, '2023-01-11', 'M', 'tamerpass', '1980-05-01', 'Giza', 'Giza', 'tamer.saad@example.com', 'd'),
('Heba', 'Ezz', 987987987, '2023-01-12', 'F', 'hebapass', '1985-09-10', 'Alexandria', 'Alexandria', 'heba.ezz@example.com', 'd'),
('Omar', 'Rashid', 456456456, '2023-01-13', 'M', 'omarpass', '1990-12-15', 'Aswan', 'Aswan', 'omar.rashid@example.com', 'd'),
('Salma', 'Khaled', 789789789, '2023-01-14', 'F', 'salmapass', '1983-03-25', 'Cairo', 'Cairo', 'salma.khaled@example.com', 'd'),
('Youssef', 'Nader', 321321321, '2023-01-15', 'M', 'youssefpass', '1987-07-30', 'Dokki', 'Giza', 'youssef.nader@example.com', 'd'),
('Hana', 'Mostafa', 654654654, '2023-01-16', 'F', 'hanapass', '1995-11-05', 'Maadi', 'Cairo', 'hana.mostafa@example.com', 'd'),
('Karim', 'Fathi', 888888888, '2023-01-17', 'M', 'karimpass', '1978-02-20', '6th of October', 'Giza', 'karim.fathi@example.com', 'd'),
('Amira', 'Zain', 555555555, '2023-01-18', 'F', 'amirapass', '1992-08-14', 'El Sheikh Zayed', 'Giza', 'amira.zain@example.com', 'd'),
('Hisham', 'Hafez', 333333333, '2023-01-19', 'M', 'hishampass', '1986-04-28', 'Alexandria', 'Alexandria', 'hisham.hafez@example.com', 'd'),
('Rana', 'Tamer', 999999999, '2023-01-20', 'F', 'ranapass', '1994-06-06', 'Heliopolis', 'Cairo', 'rana.tamer@example.com', 'd');


INSERT INTO Patient (ID, SSNValidation, PenaltyFees)
VALUES 
(1, 1, 0), 
(2, 0, 50), 
(3, 1, 20), 
(4, 1, 0), 
(5, 0, 10), 
(6, 1, 5), 
(7, 1, 15),
(8, 0, 25),
(9, 1, 0),
(10, 1, 50);




INSERT INTO Doctor (ID, PricePA, SSNValidation, Banned, SSN)
VALUES 
(11, 500, 1, 0, 123123123),
(12, 700, 1, 0, 987987987),
(13, 450, 1, 0, 456456456),
(14, 600, 1, 1, 789789789),
(15, 750, 1, 0, 321321321),
(16, 400, 1, 0, 654654654), 
(17, 550, 1, 0, 888888888), 
(18, 800, 1, 0, 555555555), 
(19, 450, 1, 0, 333333333), 
(20, 700, 1, 0, 999999999);



INSERT INTO FieldOfMedicine (FieldName, FDescription, CommonConditions)
VALUES 
('General Medicine', 'General health care', 'Flu, Minor Injuries'),
('Family Medicine', 'Comprehensive care for families', 'Flu, Hypertension'),
('Internal Medicine', 'Adult healthcare', 'Diabetes, Hypertension'),
('General Surgery', 'Surgical care', 'Appendicitis, Trauma'),
('Cardiology', 'Heart and vascular health', 'Heart Attack, Hypertension'),
('Neurology', 'Brain and nervous system', 'Stroke, Epilepsy'),
('Orthopedic Surgery', 'Bones, joints, and ligaments', 'Fractures, Arthritis'),
('Oncology', 'Cancer care', 'Breast Cancer, Lung Cancer'),
('Endocrinology', 'Hormonal and gland disorders', 'Diabetes, Thyroid Diseases'),
('Pulmonology', 'Lung and respiratory health', 'Asthma, COPD'),
('Gastroenterology', 'Digestive system care', 'Ulcers, IBS'),
('Hematology', 'Blood disorders', 'Anemia, Hemophilia'),
('Nephrology', 'Kidney health', 'Chronic Kidney Disease, Kidney Stones'),
('Urological Surgery', 'Urinary tract and male reproductive system', 'Prostate Cancer, UTIs'),
('Vascular Surgery', 'Blood vessels and lymphatic system', 'Varicose Veins, Aneurysms'),
('Plastic Surgery', 'Reconstructive and cosmetic surgery', 'Burn Reconstruction, Rhinoplasty'),
('Palliative Care', 'Care for serious illness', 'Chronic Pain, End-of-Life Care');



INSERT INTO DoctorCurWorkplace (City, Governorate, Institution, JobPosition, DoctorID)
VALUES 
('Cairo', 'Cairo', 'Cairo General Hospital', 'Cardiologist', 11),
('Giza', 'Giza', 'Giza Medical Center', 'Pediatrician', 12),
('Alexandria', 'Alexandria', 'Alexandria Health Institute', 'Neurologist', 13),
('Aswan', 'Aswan', 'Aswan Specialized Clinic', 'Surgeon', 14),
('Maadi', 'Cairo', 'Maadi Family Health', 'General Practitioner', 15),
('Heliopolis', 'Cairo', 'Heliopolis Medical Center', 'Dermatologist', 16),
('Nasr City', 'Cairo', 'Nasr City Hospital', 'Orthopedist', 17),
('6th of October', 'Giza', 'October Polyclinic', 'Gynecologist', 18),
('El Sheikh Zayed', 'Giza', 'Sheikh Zayed Clinic', 'Radiologist', 19),
('Dokki', 'Giza', 'Dokki Specialist Hospital', 'Oncologist', 20);





INSERT INTO DoctorExperience (DoctorID, Institution, Proof, SpanYears, SpanMonths, JobPosition)
VALUES 
(11, 'Cairo University', NULL, 10, 1, 'Senior Cardiologist'),
(12, 'Ain Shams University', NULL, 8, 3, 'Pediatrics Specialist'),
(13, 'Alexandria University', NULL, 7, 6, 'Neurology Fellow'),
(14, 'Aswan Medical College', NULL, 12, 3, 'Senior Surgeon'),
(15, 'Maadi Hospital', NULL, 6, 2, 'Family Medicine Practitioner'),
(16, 'Heliopolis Dermatology Center', NULL, 9, 1, 'Dermatology Consultant'),
(17, 'Nasr Orthopedic Institute', NULL, 8, 8, 'Orthopedics Specialist'),
(18, 'October Women’s Health Center', NULL, 5, 10, 'Gynecology Consultant'),
(19, 'Sheikh Zayed Diagnostic Labs', NULL, 4, 6, 'Radiology Specialist'),
(20, 'Dokki Oncology Center', NULL, 11, 5, 'Oncology Consultant');




INSERT INTO DoctorCertificate (DoctorID, date_of_acq, Institute, Description)
VALUES 
(11, '2013-06-10', 'Cairo University', 'Board Certification in Cardiology'),
(12, '2015-05-20', 'Ain Shams University', 'Pediatrics Fellowship Certification'),
(13, '2017-04-15', 'Alexandria University', 'Neurology Residency Certificate'),
(14, '2010-09-12', 'Aswan Medical College', 'Master’s in General Surgery'),
(15, '2016-02-18', 'Maadi Hospital', 'Family Medicine Specialist Certificate'),
(16, '2014-11-25', 'Heliopolis Dermatology Center', 'Board Certification in Dermatology'),
(17, '2018-07-30', 'Nasr Orthopedic Institute', 'Fellowship in Orthopedics'),
(18, '2019-08-22', 'October Women’s Health Center', 'Gynecology Consultant Certification'),
(19, '2020-01-12', 'Sheikh Zayed Diagnostic Labs', 'Certification in Diagnostic Radiology'),
(20, '2011-03-05', 'Dokki Oncology Center', 'Advanced Oncology Certification');





INSERT INTO Symptom (PatientID, [type], DateOfFirstInstance, Severity, OnsetDuration_years, OnsetDuration_months, OnsetDuration_days, IsPresent)
VALUES 
(1, 'Fever', '2023-06-01', 2, 0, 1, 10, 1),
(2, 'Cough', '2023-06-10', 3, 0, 0, 20, 1),
(3, 'Headache', '2023-07-15', 1, 0, 0, 5, 1),
(4, 'Back pain', '2023-07-20', 3, 1, 0, 12, 1),
(5, 'Nausea', '2023-08-01', 2, 0, 2, 7, 1),
(6, 'Fatigue', '2023-08-10', 1, 0, 0, 15, 1),
(7, 'Dizziness', '2023-08-20', 2, 0, 1, 10, 1),
(8, 'Blurred vision', '2023-09-15', 3, 0, 2, 10, 1);



INSERT INTO LongTermConditions (PatientID, [type], Severity, DateOfFirstInstance)
VALUES 
(8, 'Hypertension', 3, '2015-08-01'),
(2, 'Pulmonary Fibrosis', 2, '2020-02-15'),
(7, 'Leukemia', 1, '2018-01-10'),
(4, 'HIV/AIDS', 2, '2016-05-20'),
(5, 'Parkinson’s Disease', 3, '2017-11-30');




INSERT INTO Appointment (DoctorID, PatientID, IsFinished, IsConfirmed, DatenTime)
VALUES 
(11, 1, 0, 1, '2024-11-25 10:00:00'),
(12, 2, 1, 1, '2024-11-26 15:30:00'),
(13, 3, 0, 0, '2024-11-27 14:00:00'),
(14, 4, 1, 1, '2024-11-28 11:45:00'),
(15, 5, 0, 1, '2024-11-29 09:30:00'),
(16, 6, 1, 0, '2024-12-01 13:30:00'),
(17, 7, 1, 1, '2024-12-02 14:00:00'),
(18, 8, 0, 1, '2024-12-03 15:00:00'),
(19, 9, 1, 1, '2024-12-04 08:45:00'),
(20, 10, 1, 1, '2024-12-05 12:00:00');


INSERT INTO Diagnosis (AppointmentID, DoctorID, PatientID, [condition], [description])
VALUES 
(1, 11, 1, 'Flu', 'Prescribed rest and hydration.'),
(2, 12, 2, 'Diabetes Checkup', 'Adjusted insulin dosage.'),
(3, 13, 3, 'Asthma', 'Recommended inhaler use.'),
(4, 14, 4, 'Back Pain', 'Prescribed physiotherapy sessions.'),
(5, 15, 5, 'Nausea', 'Advised dietary changes.');




INSERT INTO Reviews (DoctorID, PatientID, NStars, Comment)
VALUES 
(11, 1, 5, 'Great doctor, very knowledgeable.'),
(12, 2, 4, 'Helpful and professional.'),
(13, 3, 3, 'Average experience.'),
(14, 4, 5, 'Highly recommended!'),
(15, 5, 4, 'Very kind and understanding.');
