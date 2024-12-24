use master
create database clinicdb
use clinicdb

create table [user]
(
	ID int IDENTITY(1,1),
	FName VARCHAR(20) NOT NULL,
	LName VARCHAR(20) NOT NULL,
	[type] char NOT NULL,
	SSN bigint unique not null,
	SSNValidation BIT,
    SSNPicture VARCHAR(150),
	RegistrationDate Date NOT NULL,
	Gender CHAR,
	[Password] VARCHAR(40),
	BirthDate Date,
	City VARCHAR(50),
	Governorate VARCHAR(50),
	Email VARCHAR(50),
	ProfileImageUrl Varchar(150),
	PRIMARY KEY(ID),
	UNIQUE(Email),
);

CREATE TABLE Patient
(
	ID int,
	PenaltyFees int,
	PRIMARY KEY (ID),
	FOREIGN KEY (ID) references [user],

);

CREATE TABLE Doctor 
(
	ID int,
	PricePA int,
	Banned BIT,
	PRIMARY KEY (ID),
	FOREIGN KEY (ID) references [user],
);

CREATE TABLE DoctorCurWorkplace
(
	City VARCHAR(50),
	Governorate VARCHAR(50),
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
	Proof VARCHAR(150),
	SpanYears int CHECK (SpanYears <= 80 and SpanYears >=0),
	SpanMonths int CHECK (SpanMonths <= 12 and SpanMonths > 0),
	JobPosition VARCHAR(40),
	primary key(ExpID, DoctorID),
	foreign key (DoctorID) references Doctor,
)

CREATE TABLE DoctorCertificate
(
	CertID int IDENTITY(1,1),
    CertPic VARCHAR(150) not null,
	DoctorID int,
	date_of_acq date,
    cert_validation bit,
	Institute Varchar(40),
	[Description] VarChar(80),
	PRIMARY KEY (CertID, DoctorID),
	foreign key (DoctorID) references Doctor
);

CREATE TABLE FieldOfMedicine
(
	FieldCode int IDENTITY(1,1),
	FieldName VARCHAR(60),
	FDescription VARCHAR(50),
	CommonConditions VARCHAR(50),
	PRIMARY KEY (FieldCode)
);

ALTER TABLE Doctor
ADD FieldCode int,
Constraint fk_fieldcode Foreign KEY (FieldCode) REFERENCES FieldOFMedicine


CREATE TABLE SymptomTypes (
    SymptomID INT identity(1,1) PRIMARY KEY,
    SymptomName VARCHAR(50) NOT NULL
);

INSERT INTO SymptomTypes (SymptomName)
VALUES 
    ('Hypertension'), ('Acute infections'), ('Routine screenings'), ('Chronic diseases'),
    ('Preventive care'), ('Fractures'), ('Burn repair'), ('Congenital defects'), ('Aneurysms'), ('Kidney stones'), ('Heart attack'), ('Arrhythmias'), ('Asthma'), ('Cough'), 
    ('Shortness of breath'), ('Chest pain'), ('Pain'), ('Sepsis'), ('Back pain'), 
    ('Neuropathic pain'), ('Stroke'), ('Dementia'), ('Osteoporosis'), ('Sprains'), 
    ('Stress fractures'), ('Fatigue'), ('Fever'), ('Sore throat'), ('Headache'), 
    ('Abdominal pain'), ('Nausea'), ('Vomiting'), ('Diarrhea'), ('Constipation'), 
    ('Dizziness'), ('Loss of appetite'), ('Weight loss'), ('Weight gain'), ('Joint pain'), 
    ('Swelling'), ('Itching'), ('Rash'), ('Blurred vision'), ('Hearing loss'), 
    ('Runny nose'), ('Nasal congestion'), ('Muscle pain'), ('Night sweats'), 
    ('Palpitations'), ('Tingling'), ('Numbness'), ('Weakness'), ('Difficulty breathing'), 
    ('Difficulty swallowing'), ('Confusion'), ('Irritability'), ('Excessive thirst'), 
    ('Frequent urination'), ('Hair loss'), ('Sweating'), ('Chills'), ('Yellowing of the skin (jaundice)');

CREATE TABLE Symptom
(
	PatientID int,
	SymptomID int,
	DateOfFirstInstance Date,
	Severity int,
	OnsetDuration_years int,
	OnsetDuration_months int,
	OnsetDuration_days int,
	IsPresent BIT,
	primary key(PatientID, SymptomID),
	foreign key (PatientID) references Patient,
    foreign key (SymptomID) references SymptomTypes,
);


CREATE TABLE LTCTypes (
    ConditionID INT IDENTITY(1,1) PRIMARY KEY,
    ConditionName VARCHAR(100) NOT NULL
);

INSERT INTO LTCTypes (ConditionName)
VALUES
    ('Hypertension'), ('Coronary Artery Disease'), ('Heart Failure'), ('Arrhythmia'),
    ('Stroke'), ('Peripheral Artery Disease'), ('Asthma'), ('Chronic Obstructive Pulmonary Disease (COPD)'),
    ('Sleep Apnea'), ('Pulmonary Fibrosis'), ('Alzheimer’s Disease'), ('Parkinson’s Disease'),
    ('Multiple Sclerosis'), ('Epilepsy'), ('Depression'), ('Bipolar Disorder'),
    ('Schizophrenia'), ('Anxiety Disorders'), ('Diabetes Type 1'), ('Diabetes Type 2'),
    ('Hypothyroidism'), ('Hyperthyroidism'), ('Polycystic Ovary Syndrome (PCOS)'),
    ('Cushing''s Syndrome'), ('Inflammatory Bowel Disease (IBD)'), ('Crohn’s Disease'),
    ('Ulcerative Colitis'), ('Irritable Bowel Syndrome (IBS)'), ('Celiac Disease'),
    ('Gastroesophageal Reflux Disease (GERD)'), ('Osteoarthritis'), ('Rheumatoid Arthritis'),
    ('Osteoporosis'), ('Gout'), ('Ankylosing Spondylitis'), ('Psoriasis'), ('Eczema'),
    ('Vitiligo'), ('Lupus'), ('Breast Cancer'), ('Lung Cancer'), ('Prostate Cancer'),
    ('Colorectal Cancer'), ('Leukemia'), ('Lymphoma'), ('Systemic Lupus Erythematosus (SLE)'),
    ('Sjögren’s Syndrome'), ('Hashimoto’s Thyroiditis'), ('Chronic Kidney Disease (CKD)'),
    ('Polycystic Kidney Disease'), ('Kidney Stones'), ('HIV/AIDS'), ('Chronic Fatigue Syndrome'),
    ('Fibromyalgia'), ('Sickle Cell Disease'), ('Hemophilia'), ('Thalassemia');

Create Table LongTermConditions
(
	PatientID int,
	ConditionID int,
	Severity int,
	DateOfFirstInstance Date,
	primary key(PatientID, ConditionID),
	foreign key (PatientID) references Patient,
    foreign key (ConditionID) references LTCTypes,
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
	prescription varchar(1000),
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


CREATE TABLE AllowedGovernorates
(
    Governorate VARCHAR(50) PRIMARY KEY
);

CREATE TABLE AllowedCities
(
    City VARCHAR(50),
    Governorate VARCHAR(50),
    FOREIGN KEY (Governorate) REFERENCES AllowedGovernorates(Governorate),
    PRIMARY KEY (City, Governorate)
);

-- Populate it with your allowed governorates
INSERT INTO AllowedGovernorates (Governorate)
VALUES
    ('Cairo'), ('Giza'), ('Alexandria'), ('Aswan'), ('Asyut'),
    ('Beheira'), ('Beni Suef'), ('Dakahlia'), ('Damietta'), ('Faiyum'),
    ('Gharbia'), ('Ismailia'), ('Kafr El Sheikh'), ('Luxor'), ('Matrouh'),
    ('Minya'), ('Monufia'), ('New Valley'), ('North Sinai'), ('Port Said'),
    ('Qalyubia'), ('Qena'), ('Red Sea'), ('Sharqia'), ('Sohag'),
    ('South Sinai'), ('Suez');


-- Populate it with the allowed cities for each governorate
INSERT INTO AllowedCities (City, Governorate)
VALUES
    ('Cairo', 'Cairo'), ('Nasr City', 'Cairo'), ('Heliopolis', 'Cairo'),
    ('Maadi', 'Cairo'), ('Shobra', 'Cairo'), ('Zamalek', 'Cairo'),
    ('Giza', 'Giza'), ('6th of October', 'Giza'), ('El Sheikh Zayed', 'Giza'),
    ('Haram', 'Giza'), ('Dokki', 'Giza'),
    ('Alexandria', 'Alexandria'), ('Borg El Arab', 'Alexandria'),
    ('Aswan', 'Aswan'), ('Edfu', 'Aswan'), ('Kom Ombo', 'Aswan'),
    ('Asyut', 'Asyut'), ('Dayrout', 'Asyut'), ('Manfalut', 'Asyut'),
    ('Damanhour', 'Beheira'), ('Kafr El Dawwar', 'Beheira'), ('Edku', 'Beheira'),
    ('Beni Suef', 'Beni Suef'), ('Al Fashn', 'Beni Suef'), ('Ihnasia', 'Beni Suef'),
    ('Mansoura', 'Dakahlia'), ('Talkha', 'Dakahlia'), ('Mit Ghamr', 'Dakahlia'),
    ('Damietta', 'Damietta'), ('Ras El Bar', 'Damietta'), ('Faraskur', 'Damietta'),
    ('Faiyum', 'Faiyum'), ('Senoures', 'Faiyum'), ('Itsa', 'Faiyum'),
    ('Tanta', 'Gharbia'), ('El Mahalla El Kubra', 'Gharbia'), ('Kafr El Zayat', 'Gharbia'),
    ('Ismailia', 'Ismailia'), ('Fayed', 'Ismailia'), ('Qantara', 'Ismailia'),
    ('Kafr El Sheikh', 'Kafr El Sheikh'), ('Baltim', 'Kafr El Sheikh'), ('Desouk', 'Kafr El Sheikh'),
    ('Luxor', 'Luxor'), ('Armant', 'Luxor'), ('Esna', 'Luxor'),
    ('Marsa Matrouh', 'Matrouh'), ('Sidi Abdel Rahman', 'Matrouh'), ('El Alamein', 'Matrouh'),
    ('Minya', 'Minya'), ('Beni Mazar', 'Minya'), ('Mallawi', 'Minya'),
    ('Shibin El Kom', 'Monufia'), ('Menouf', 'Monufia'), ('Quesna', 'Monufia'),
    ('Kharga', 'New Valley'), ('Dakhla', 'New Valley'),
    ('Arish', 'North Sinai'), ('Sheikh Zuwayed', 'North Sinai'), ('Rafah', 'North Sinai'),
    ('Port Said', 'Port Said'), ('Port Fouad', 'Port Said'),
    ('Benha', 'Qalyubia'), ('Shubra El Kheima', 'Qalyubia'), ('Kafr Shukr', 'Qalyubia'),
    ('Qena', 'Qena'), ('Nag Hammadi', 'Qena'), ('Qus', 'Qena'),
    ('Hurghada', 'Red Sea'), ('Safaga', 'Red Sea'), ('Marsa Alam', 'Red Sea'),
    ('Zagazig', 'Sharqia'), ('Belbeis', 'Sharqia'), ('10th of Ramadan', 'Sharqia'),
    ('Sohag', 'Sohag'), ('Akhmim', 'Sohag'), ('Girga', 'Sohag'),
    ('Sharm El Sheikh', 'South Sinai'), ('Dahab', 'South Sinai'), ('El Tor', 'South Sinai'),
    ('Suez', 'Suez'), ('Ain Sokhna', 'Suez');

ALTER TABLE [user]
ADD CONSTRAINT FK_Governorate FOREIGN KEY (Governorate)
    REFERENCES AllowedGovernorates (Governorate);

ALTER TABLE [user]
ADD CONSTRAINT FK_City FOREIGN KEY (City, Governorate)
    REFERENCES AllowedCities (City, Governorate);


-- Populating FieldOfMedicine table.
INSERT INTO FieldOfMedicine (FieldName, FDescription, CommonConditions)
VALUES
    ('General Medicine', 'Broad medical practice', 'Hypertension, Diabetes'),
    ('Family Medicine', 'Comprehensive care for all ages', 'Acute infections, Routine screenings'),
    ('Internal Medicine', 'Adult medical care', 'Chronic diseases, Preventive care'),
    ('General Surgery', 'Surgical treatment of conditions', 'Hernias, Appendicitis'),
    ('Cardiothoracic Surgery', 'Heart and lung surgery', 'Heart valve disease, Lung cancer'),
    ('Neurosurgery', 'Brain and spinal surgery', 'Brain tumors, Spinal cord injuries'),
    ('Orthopedic Surgery', 'Bone and joint surgery', 'Fractures, Arthritis'),
    ('Plastic Surgery', 'Reconstructive and cosmetic surgery', 'Burn repair, Rhinoplasty'),
    ('Pediatric Surgery', 'Surgery for children', 'Congenital defects, Appendicitis in children'),
    ('Vascular Surgery', 'Blood vessel surgery', 'Aneurysms, Varicose veins'),
    ('Urological Surgery', 'Urinary system surgery', 'Kidney stones, Prostate cancer'),
    ('Cardiology', 'Heart health management', 'Heart attack, Arrhythmias'),
    ('Endocrinology', 'Hormonal disorders', 'Diabetes, Thyroid disorders'),
    ('Gastroenterology', 'Digestive system health', 'Irritable bowel syndrome, Ulcers'),
    ('Hematology', 'Blood disorders', 'Anemia, Leukemia'),
    ('Nephrology', 'Kidney health management', 'Chronic kidney disease, Kidney stones'),
    ('Oncology', 'Cancer diagnosis and treatment', 'Breast cancer, Lung cancer'),
    ('Pulmonology', 'Lung and respiratory care', 'Asthma, COPD'),
    ('Rheumatology', 'Joint and autoimmune diseases', 'Rheumatoid arthritis, Lupus'),
    ('Infectious Diseases', 'Management of infections', 'HIV/AIDS, Tuberculosis'),
    ('General Pediatrics', 'Child health management', 'Vaccinations, Growth disorders'),
    ('Neonatology', 'Care for newborns', 'Premature birth, Neonatal sepsis'),
    ('Pediatric Cardiology', 'Heart health in children', 'Congenital heart defects, Arrhythmias in children'),
    ('Pediatric Neurology', 'Neurological care in children', 'Epilepsy, Cerebral palsy'),
    ('Obstetrics', 'Pregnancy and childbirth', 'Prenatal care, High-risk pregnancy'),
    ('Gynecology', 'Women’s reproductive health', 'Endometriosis, Ovarian cysts'),
    ('Maternal-Fetal Medicine', 'High-risk pregnancies', 'Preeclampsia, Gestational diabetes'),
    ('Reproductive Endocrinology', 'Fertility treatments', 'Infertility, Polycystic ovary syndrome'),
    ('Emergency Medicine', 'Acute medical care', 'Trauma, Heart attack'),
    ('Critical Care Medicine', 'Life-threatening conditions', 'Sepsis, Respiratory failure'),
    ('Trauma Medicine', 'Emergency injury care', 'Fractures, Internal bleeding'),
    ('Anesthesiology', 'Pain management during procedures', 'Anesthesia for surgery, Pain control'),
    ('Pain Medicine', 'Chronic pain management', 'Back pain, Neuropathic pain'),
    ('Neurology', 'Brain and nervous system care', 'Stroke, Multiple sclerosis'),
    ('Psychiatry', 'Mental health management', 'Depression, Anxiety disorders'),
    ('Child and Adolescent Psychiatry', 'Mental health in youth', 'ADHD, Autism spectrum disorders'),
    ('Diagnostic Radiology', 'Imaging for diagnosis', 'X-rays, MRI scans'),
    ('Interventional Radiology', 'Minimally invasive procedures', 'Angioplasty, Tumor ablation'),
    ('Nuclear Medicine', 'Radioactive imaging and treatment', 'Thyroid cancer, Bone scans'),
    ('Anatomical Pathology', 'Study of tissue for diagnosis', 'Cancer biopsies, Autopsies'),
    ('Clinical Pathology', 'Laboratory diagnostics', 'Blood tests, Urine analysis'),
    ('Forensic Pathology', 'Post-mortem examinations', 'Homicide investigations, Accidental deaths'),
    ('General Dermatology', 'Skin health management', 'Eczema, Acne'),
    ('Cosmetic Dermatology', 'Aesthetic skin treatments', 'Botox, Laser treatments'),
    ('Ophthalmology', 'Eye health management', 'Cataracts, Glaucoma'),
    ('Otolaryngology (ENT)', 'Ear, nose, and throat care', 'Sinusitis, Hearing loss'),
    ('Public Health', 'Community health and prevention', 'Disease outbreaks, Health education'),
    ('Occupational Medicine', 'Workplace health', 'Workplace injuries, Occupational asthma'),
    ('Preventive Medicine', 'Health risk reduction', 'Vaccinations, Lifestyle modifications'),
    ('Geriatrics', 'Elderly care', 'Dementia, Osteoporosis'),
    ('Sports Medicine', 'Injuries in athletes', 'Sprains, Stress fractures'),
    ('Addiction Medicine', 'Substance abuse treatment', 'Alcohol addiction, Opioid dependency'),
    ('Palliative Care', 'End-of-life and chronic care', 'Cancer pain, Advanced COPD'),
    ('Medical Genetics', 'Genetic disorders', 'Cystic fibrosis, Genetic counseling');







INSERT INTO [user] (FName, LName, SSN, SSNValidation, RegistrationDate, Gender, [Password], BirthDate, City, Governorate, Email, [type], ProfileImageUrl)
VALUES 

-- Patients
('John', 'Doe', 123456789, 0, '2023-01-01', 'M', 'password123', '1985-05-15', 'Cairo', 'Cairo', 'john.doe@example.com', 'p', 'author1.jpg'),
('Jane', 'Smith', 987654321, 1, '2023-01-02', 'F', 'securepass', '1990-07-20', 'Giza', 'Giza', 'jane.smith@example.com', 'p', 'author1.jpg'),
('Ahmed', 'Ali', 223344556, 1, '2023-01-03', 'M', 'ahmedpass', '1975-09-12', 'Alexandria', 'Alexandria', 'ahmed.ali@example.com', 'p', 'author1.jpg'),
('Sara', 'Hassan', 445566778, 1, '2023-01-04', 'F', 'sarapass', '1992-03-25', 'Aswan', 'Aswan', 'sara.hassan@example.com', 'p', 'author1.jpg'),
('Mohamed', 'Youssef', 112233445, 1, '2023-01-05', 'M', 'mypassword', '1987-09-18', 'Zamalek', 'Cairo', 'mohamed.youssef@example.com', 'p', 'author1.jpg'),
('Nora', 'Farid', 998877665, 0, '2023-01-06', 'F', 'norapass', '1993-11-25', 'Heliopolis', 'Cairo', 'nora.farid@example.com', 'p', 'author1.jpg'),
('Ali', 'Kamal', 667788990, 1, '2023-01-07', 'M', 'alikpass', '1988-03-19', 'Nasr City', 'Cairo', 'ali.kamal@example.com', 'p', 'author1.jpg'),
('Mona', 'Gamal', 445566334, 1, '2023-01-08', 'F', 'monag123', '1991-06-14', 'Maadi', 'Cairo', 'mona.gamal@example.com', 'p', 'author1.jpg'),
('Hassan', 'Tariq', 332211009, 0, '2023-01-09', 'M', 'hassanpass', '1979-12-11', 'Shobra', 'Cairo', 'hassan.tariq@example.com', 'p', 'author1.jpg'),
('Layla', 'Nashat', 776655443, 1, '2023-01-10', 'F', 'laylapass', '1995-02-28', 'Zamalek', 'Cairo', 'layla.nashat@example.com', 'p', 'author1.jpg'),

-- Doctors
('Tamer', 'Saad', 123123123, 1, '2023-01-11', 'M', 'tamerpass', '1980-05-01', 'Giza', 'Giza', 'tamer.saad@example.com', 'd', 'author1.jpg'),
('Heba', 'Ezz', 987987987, 0, '2023-01-12', 'F', 'hebapass', '1985-09-10', 'Alexandria', 'Alexandria', 'heba.ezz@example.com', 'd', 'author1.jpg'),
('Omar', 'Rashid', 456456456, 1, '2023-01-13', 'M', 'omarpass', '1990-12-15', 'Aswan', 'Aswan', 'omar.rashid@example.com', 'd', 'author1.jpg'),
('Salma', 'Khaled', 789789789, 1, '2023-01-14', 'F', 'salmapass', '1983-03-25', 'Cairo', 'Cairo', 'salma.khaled@example.com', 'd', 'author1.jpg'),
('Youssef', 'Nader', 321321321, 0, '2023-01-15', 'M', 'youssefpass', '1987-07-30', 'Dokki', 'Giza', 'youssef.nader@example.com', 'd', 'author1.jpg'),
('Hana', 'Mostafa', 654654654, 1, '2023-01-16', 'F', 'hanapass', '1995-11-05', 'Maadi', 'Cairo', 'hana.mostafa@example.com', 'd', 'author1.jpg'),
('Karim', 'Fathi', 888888888, 1, '2023-01-17', 'M', 'karimpass', '1978-02-20', '6th of October', 'Giza', 'karim.fathi@example.com', 'd', 'author1.jpg'),
('Amira', 'Zain', 555555555, 1, '2023-01-18', 'F', 'amirapass', '1992-08-14', 'El Sheikh Zayed', 'Giza', 'amira.zain@example.com', 'd', 'author1.jpg'),
('Hisham', 'Hafez', 333333333, 1,'2023-01-19', 'M', 'hishampass', '1986-04-28', 'Alexandria', 'Alexandria', 'hisham.hafez@example.com', 'd', 'author1.jpg'),
('Rana', 'Tamer', 999999999, 1, '2023-01-20', 'F', 'ranapass', '1994-06-06', 'Heliopolis', 'Cairo', 'rana.tamer@example.com', 'd', 'author1.jpg'),

-- Admin
('admin', 'admin', 00000001, 1, '0001-01-01', 'M', 'adminpass', '0001-01-01', 'Giza', 'Giza', 'admin@gmail.com', 'a', 'author1.jpg');




INSERT INTO Patient (ID, PenaltyFees)
VALUES 
(1, 0), 
(2, 50), 
(3, 20), 
(4, 0), 
(5, 10), 
(6, 5), 
(7, 15),
(8, 25),
(9, 0),
(10, 50);




INSERT INTO Doctor (ID, PricePA, Banned, FieldCode)
VALUES 
(11, 500, 0, 1),
(12, 700, 0, 2),
(13, 450, 0, 3),
(14, 600, 1, 4),
(15, 750, 0, 5),
(16, 400, 0, 6), 
(17, 550, 0, 1), 
(18, 800, 0, 2), 
(19, 450, 0, 7), 
(20, 700, 0, 9);




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



INSERT INTO DoctorCertificate (DoctorID, date_of_acq, cert_validation, Institute, [Description])
VALUES 
(11, '2013-06-10', 1, 'Cairo University', 'Board Certification in Cardiology'),
(12, '2015-05-20', 0, 'Ain Shams University', 'Pediatrics Fellowship Certification'),
(13, '2017-04-15', 1, 'Alexandria University', 'Neurology Residency Certificate'),
(14, '2010-09-12', 0, 'Aswan Medical College', 'Master’s in General Surgery'),
(15, '2016-02-18', 1, 'Maadi Hospital', 'Family Medicine Specialist Certificate'),
(16, '2014-11-25', 0, 'Heliopolis Dermatology Center', 'Board Certification in Dermatology'),
(17, '2018-07-30', 1, 'Nasr Orthopedic Institute', 'Fellowship in Orthopedics'),
(18, '2019-08-22', 0, 'October Women’s Health Center', 'Gynecology Consultant Certification'),
(19, '2020-01-12', 1, 'Sheikh Zayed Diagnostic Labs', 'Certification in Diagnostic Radiology'),
(20, '2011-03-05', 0, 'Dokki Oncology Center', 'Advanced Oncology Certification');


INSERT INTO Symptom (PatientID, SymptomID, DateOfFirstInstance, Severity, OnsetDuration_years, OnsetDuration_months, OnsetDuration_days, IsPresent)
VALUES
    (1, 1, '2023-02-01', 3, 0, 11, 5, 1), -- Hypertension, ongoing symptom
    (2, 5, '2023-10-15', 2, 0, 1, 10, 1), -- Cough, recent onset
    (3, 12, '2022-05-30', 4, 1, 6, 15, 1), -- Joint pain, chronic symptom
    (4, 25, '2024-01-01', 1, 0, 0, 12, 1), -- Fatigue, mild
    (5, 30, '2021-08-12', 5, 2, 4, 0, 0), -- Nausea, resolved
    (6, 17, '2023-09-10', 3, 0, 3, 20, 1), -- Rash, ongoing
    (7, 8, '2020-11-20', 2, 3, 0, 10, 1), -- Shortness of breath, long-term symptom
    (8, 18, '2023-04-05', 3, 0, 8, 0, 1), -- Abdominal pain, persistent
    (9, 35, '2023-07-15', 4, 0, 6, 25, 1), -- Dizziness, recent
    (10, 40, '2024-02-01', 2, 0, 0, 10, 1); -- Headache, acute and mild


INSERT INTO LongTermConditions (PatientID, ConditionID, Severity, DateOfFirstInstance)
VALUES
    (1, 1, 2, '2020-03-15'), -- Hypertension
    (1, 8, 3, '2019-05-10'), -- Asthma

    (2, 14, 4, '2018-11-22'), -- Depression

    (3, 3, 2, '2021-07-01'), -- Heart Failure
    (3, 31, 3, '2020-02-14'), -- Osteoarthritis

    (5, 40, 4, '2016-09-30'), -- Breast Cancer
    (5, 42, 3, '2018-12-05'), -- Prostate Cancer

    (8, 7, 2, '2022-01-20'), -- COPD
    (8, 10, 1, '2023-06-10'); -- Pulmonary Fibrosis


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
