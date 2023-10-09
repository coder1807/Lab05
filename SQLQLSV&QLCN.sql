CREATE DATABASE QLCN
GO

USE QLCN
GO

CREATE TABLE Faculty(
	FacultyID int not null primary key,
	FacultyName nvarchar(255) not null
)
go

CREATE TABLE Major(
	FacultyID int not null,
	MajorID int not null,
	Name nvarchar(255) not null,
	primary key(FacultyID,MajorID),

	foreign key (FacultyID) references Faculty(FacultyID)
)
GO

CREATE TABLE Student (
	StudentID nvarchar(10) not null primary key,
	FullName nvarchar(255) not null,
	AverageScore float not null,
	FacultyID int null,
	MajorID int null,
	Avatar nvarchar(255) null,

	foreign key (FacultyID) references Faculty(FacultyID),
	foreign key (FacultyID,MajorID) references Major(FacultyID,MajorID)
	
)
GO

INSERT INTO Faculty(FacultyID, FacultyName)
VALUES ('1',N'Công Nghệ Thông Tin')
INSERT INTO Faculty(FacultyID, FacultyName)
VALUES ('2',N'Ngôn Ngữ Anh')
INSERT INTO Faculty(FacultyID, FacultyName)
VALUES ('3',N'Quản Trị Kinh Doanh')

INSERT INTO Major(FacultyID, MajorID,Name)
VALUES ('1','1',N'Công Nghệ Phần Mềm')
INSERT INTO Major(FacultyID, MajorID,Name)
VALUES ('1','2',N'Hệ Thống Thông Tin')
INSERT INTO Major(FacultyID, MajorID,Name)
VALUES ('1','3',N'An Toàn Thông Tin')
INSERT INTO Major(FacultyID, MajorID,Name)
VALUES ('2','1',N'Tiếng Anh Thương Mại')
INSERT INTO Major(FacultyID, MajorID,Name)
VALUES ('2','2',N'Tiếng Anh Truyền Thông')
INSERT INTO Major(FacultyID, MajorID,Name)
VALUES ('3','1',N'Kinh Doanh Quốc Tế')
INSERT INTO Major(FacultyID, MajorID,Name)
VALUES ('3','2',N'Quản Trị Nhân Sự')

INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180608218', N'Bùi Hoàng Việt', '1', '9.5', '1', null)
INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180608217', N'Bùi Huy Hoàng', '1', '8.5', null, null)
INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180608216', N'Nguyễn Đức Hòa', '1', '9.5', null, null)
INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180606789', N'Dương Văn Minh', '1', '4.5', '2', null)
INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180601234', N'Nguyễn Quang Huy', '2', '10', null, null)
INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180601235', N'Nguyễn Huy Hoàng', '2', '7', '1', null)
INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180601236', N'Trần Văn Cao', '2', '8', '2', null)
INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180601237', N'Trần Cao Minh', '2', '8.5', null, null)
INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180600123', N'Phan Huy Lưu', '3', '10', '1', null)
INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180600124', N'Dương Thành Phết', '3', '9', '2', null)
INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180600125', N'Trần Thị Lụa', '3', '10', null, null)
INSERT INTO Student(StudentID, FullName, FacultyID, AverageScore, MajorID, Avatar)
VALUES('2180600126', N'Nguyễn Thị Ngọc Anh', '3', '8', null, null)
