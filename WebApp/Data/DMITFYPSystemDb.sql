USE [DMITFYPSystemDb]
GO
/****** Object:  Table [dbo].[Announcement]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Announcement](
    [announcement_id] [int] IDENTITY(1,1) NOT NULL,
    [announcement_date] [datetime] NOT NULL,
    [message] [varchar](1000) NOT NULL,
    [title] [varchar](100) NOT NULL,
    [last_edit_date] [datetime] NOT NULL,
    [lecturer_id] [int] NOT NULL,
    [course_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [announcement_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Course]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Course](
    [course_id] [int] IDENTITY(1,1) NOT NULL,
    [course_full_name] [varchar](50) NOT NULL,
    [course_acronym] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [course_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[File_Resource]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[File_Resource](
    [file_resource_id] [int] IDENTITY(1,1) NOT NULL,
    [name] [varchar](100) NOT NULL,
    [extension] [varchar](10) NOT NULL,
    [data] [varbinary](max) NOT NULL,
    [announcement_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [file_resource_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Group]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Group](
    [group_id] [int] IDENTITY(1,1) NOT NULL,
    [group_number] [char](2) NOT NULL,
    [valid] [bit] NOT NULL,
    [lecturer_id] [int] NULL,
    [project_id] [int] NULL,
    [course_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [group_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Group_Joining_Request]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group_Joining_Request](
    [group_joining_request_id] [int] IDENTITY(1,1) NOT NULL,
    [request_date] [datetime] NOT NULL,
    [request_acceptance_date] [datetime] NULL,
    [student_id] [int] NOT NULL,
    [group_id] [int] NOT NULL,
    [notified_of_group_joining_request_outcome] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
    [group_joining_request_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Industry_Lecturer_Project]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Industry_Lecturer_Project](
    [project_id] [int] NOT NULL,
    [project_milestones] [varchar](500) NULL,
    [company_name] [varchar](50) NULL,
    [company_postal_address] [varchar](100) NULL,
    [company_telephone_number] [char](8) NULL,
    [company_fax_number] [char](8) NULL,
    [company_email_address] [varchar](50) NULL,
    [company_sponsorship_willingness] [bit] NULL,
    [company_liaison_officer_name] [varchar](50) NULL,
    [deadline] [datetime] NULL,
    [lecturer_id] [int] NULL,
 CONSTRAINT [PK__Industry__BC799E1F0ABB1516] PRIMARY KEY CLUSTERED 
(
    [project_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Lecturer]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Lecturer](
    [lecturer_id] [int] IDENTITY(1,1) NOT NULL,
    [staff_id] [char](5) NOT NULL,
    [full_name] [varchar](50) NOT NULL,
    [contact_number] [char](8) NOT NULL,
    [email_address] [varchar](50) NOT NULL,
    [admin] [bit] NOT NULL,
    [course_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [lecturer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
    [staff_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Notification](
    [notification_id] [int] IDENTITY(1,1) NOT NULL,
    [type] [int] NOT NULL,
    [user_id] [varchar](7) NOT NULL,
    [date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [notification_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Project]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Project](
    [project_id] [int] IDENTITY(1,1) NOT NULL,
    [project_title] [varchar](100) NULL,
    [project_aims] [varchar](500) NULL,
    [project_objectives] [varchar](1000) NULL,
    [target_audience] [varchar](500) NULL,
    [main_functions_and_deliverables] [varchar](500) NULL,
    [hardware_and_software_requirements] [varchar](500) NULL,
    [course_id] [int] NOT NULL,
    [project_availability] [bit] NULL,
    [submitted_on] [datetime] NULL,
    [updated_at] [datetime] NULL,
 CONSTRAINT [PK__Project__BC799E1FFA7BEB4E] PRIMARY KEY CLUSTERED 
(
    [project_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Project_Choice]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project_Choice](
    [student_id] [int] NOT NULL,
    [project_id] [int] NOT NULL,
    [ranking_precedence] [int] NOT NULL,
    [submitted_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [student_id] ASC,
    [project_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Student]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Student](
    [student_id] [int] IDENTITY(1,1) NOT NULL,
    [admin_number] [char](7) NOT NULL,
    [full_name] [varchar](50) NOT NULL,
    [mobile_number] [char](8) NOT NULL,
    [email_address] [varchar](50) NOT NULL,
    [group_role] [char](1) NULL,
    [year] [int] NOT NULL,
    [semester] [int] NOT NULL,
    [completed_module] [bit] NOT NULL,
    [group_id] [int] NULL,
    [course_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [student_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
    [admin_number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Student_Project]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Student_Project](
    [project_id] [int] NOT NULL,
    [project_background] [varchar](500) NULL,
    [project_overview] [varchar](500) NULL,
    [strategy_and_approach] [varchar](500) NULL,
    [merits_comparison] [varchar](500) NULL,
    [commercialisation_strategy] [varchar](500) NULL,
    [project_milestones_and_workload_allocation] [varchar](500) NULL,
    [problems_and_countermeasures] [varchar](500) NULL,
    [group_id] [int] NULL,
 CONSTRAINT [PK__Student___BC799E1FB56A7CA3] PRIMARY KEY CLUSTERED 
(
    [project_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Student_Project_Extra]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Student_Project_Extra](
    [field_id] [int] IDENTITY(1,1) NOT NULL,
    [field_name] [varchar](50) NOT NULL,
    [description_text] [varchar](100) NULL,
    [text_value] [varchar](500) NOT NULL,
    [project_id] [int] NULL,
 CONSTRAINT [PK_Student_Project_Extra] PRIMARY KEY CLUSTERED 
(
    [field_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Student_Project_History]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_Project_History](
    [response_id] [int] IDENTITY(1,1) NOT NULL,
    [project_id] [int] NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF_Project_project_availability]  DEFAULT ((0)) FOR [project_availability]
GO
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF_Project_submitted_on]  DEFAULT (NULL) FOR [submitted_on]
GO
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF_Project_updated_at]  DEFAULT (NULL) FOR [updated_at]
GO
ALTER TABLE [dbo].[Announcement]  WITH CHECK ADD  CONSTRAINT [fk_announcement_course_id] FOREIGN KEY([course_id])
REFERENCES [dbo].[Course] ([course_id])
GO
ALTER TABLE [dbo].[Announcement] CHECK CONSTRAINT [fk_announcement_course_id]
GO
ALTER TABLE [dbo].[Announcement]  WITH CHECK ADD  CONSTRAINT [fk_announcement_lecturer_id] FOREIGN KEY([lecturer_id])
REFERENCES [dbo].[Lecturer] ([lecturer_id])
GO
ALTER TABLE [dbo].[Announcement] CHECK CONSTRAINT [fk_announcement_lecturer_id]
GO
ALTER TABLE [dbo].[File_Resource]  WITH CHECK ADD  CONSTRAINT [fk_file_resource_announcement_id] FOREIGN KEY([announcement_id])
REFERENCES [dbo].[Announcement] ([announcement_id])
GO
ALTER TABLE [dbo].[File_Resource] CHECK CONSTRAINT [fk_file_resource_announcement_id]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [fk_group_course_id] FOREIGN KEY([course_id])
REFERENCES [dbo].[Course] ([course_id])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [fk_group_course_id]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [fk_group_lecturer_id] FOREIGN KEY([lecturer_id])
REFERENCES [dbo].[Lecturer] ([lecturer_id])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [fk_group_lecturer_id]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [fk_group_project_id] FOREIGN KEY([project_id])
REFERENCES [dbo].[Project] ([project_id])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [fk_group_project_id]
GO
ALTER TABLE [dbo].[Group_Joining_Request]  WITH CHECK ADD  CONSTRAINT [fk_group_joining_request_group_id] FOREIGN KEY([group_id])
REFERENCES [dbo].[Group] ([group_id])
GO
ALTER TABLE [dbo].[Group_Joining_Request] CHECK CONSTRAINT [fk_group_joining_request_group_id]
GO
ALTER TABLE [dbo].[Group_Joining_Request]  WITH CHECK ADD  CONSTRAINT [fk_group_joining_request_student_id] FOREIGN KEY([student_id])
REFERENCES [dbo].[Student] ([student_id])
GO
ALTER TABLE [dbo].[Group_Joining_Request] CHECK CONSTRAINT [fk_group_joining_request_student_id]
GO
ALTER TABLE [dbo].[Industry_Lecturer_Project]  WITH CHECK ADD  CONSTRAINT [fk_industry_lecturer_project_project_id] FOREIGN KEY([project_id])
REFERENCES [dbo].[Project] ([project_id])
GO
ALTER TABLE [dbo].[Industry_Lecturer_Project] CHECK CONSTRAINT [fk_industry_lecturer_project_project_id]
GO
ALTER TABLE [dbo].[Lecturer]  WITH CHECK ADD  CONSTRAINT [fk_lecturer_course_id] FOREIGN KEY([course_id])
REFERENCES [dbo].[Course] ([course_id])
GO
ALTER TABLE [dbo].[Lecturer] CHECK CONSTRAINT [fk_lecturer_course_id]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [fk_project_course_id] FOREIGN KEY([course_id])
REFERENCES [dbo].[Course] ([course_id])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [fk_project_course_id]
GO
ALTER TABLE [dbo].[Project_Choice]  WITH CHECK ADD  CONSTRAINT [fk_project_choice_project_id] FOREIGN KEY([project_id])
REFERENCES [dbo].[Project] ([project_id])
GO
ALTER TABLE [dbo].[Project_Choice] CHECK CONSTRAINT [fk_project_choice_project_id]
GO
ALTER TABLE [dbo].[Project_Choice]  WITH CHECK ADD  CONSTRAINT [fk_project_choice_student_id] FOREIGN KEY([student_id])
REFERENCES [dbo].[Student] ([student_id])
GO
ALTER TABLE [dbo].[Project_Choice] CHECK CONSTRAINT [fk_project_choice_student_id]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [fk_student_course_id] FOREIGN KEY([course_id])
REFERENCES [dbo].[Course] ([course_id])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [fk_student_course_id]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [fk_student_group_id] FOREIGN KEY([group_id])
REFERENCES [dbo].[Group] ([group_id])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [fk_student_group_id]
GO
ALTER TABLE [dbo].[Student_Project]  WITH CHECK ADD  CONSTRAINT [fk_student_project_project_id] FOREIGN KEY([project_id])
REFERENCES [dbo].[Project] ([project_id])
GO
ALTER TABLE [dbo].[Student_Project] CHECK CONSTRAINT [fk_student_project_project_id]
GO
/****** Object:  StoredProcedure [dbo].[createGroupJoiningRequestSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[createGroupJoiningRequestSP] 
    -- Add the parameters for the stored procedure here
    @request_date DATETIME,
    @request_acceptance_date DATETIME,
    @student_id INT,
    @group_id INT,
    @notified_of_group_joining_request_outcome BIT
AS
BEGIN
    -- Insert statements for procedure here
    INSERT INTO Group_Joining_Request
    (request_date, request_acceptance_date, student_id, group_id, notified_of_group_joining_request_outcome)
    VALUES
    (@request_date, @request_acceptance_date, @student_id, @group_id, @notified_of_group_joining_request_outcome)
END
GO
/****** Object:  StoredProcedure [dbo].[createGroupSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[createGroupSP] 
    -- Add the parameters for the stored procedure here
    @group_number CHAR(2),
    @valid BIT,
    @lecturer_id INT,
    @project_id INT,
    @course_id INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    INSERT INTO "Group"
    (group_number, valid, lecturer_id, project_id, course_id)
    VALUES
    (@group_number, @valid, @lecturer_id, @project_id, @course_id)
END
GO
/****** Object:  StoredProcedure [dbo].[createLecturerSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[createLecturerSP] 
    -- Add the parameters for the stored procedure here
    @staff_id CHAR(5),
    @full_name VARCHAR(50),
    @contact_number CHAR(8),
    @email_address VARCHAR(50),
    @admin BIT,
    @course_id INT
    AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    INSERT INTO Lecturer(staff_id, full_name, contact_number, email_address, "admin", course_id)
    VALUES
    (@staff_id, @full_name, @contact_number, @email_address, @admin, @course_id)
END
GO
/****** Object:  StoredProcedure [dbo].[createProjectChoiceSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[createProjectChoiceSP] 
    -- Add the parameters for the stored procedure here
    @student_id INT,
    @project_id INT,
    @ranking_precedence INT,
    @submitted_date DATETIME
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    INSERT INTO Project_Choice
    (student_id, project_id, ranking_precedence, submitted_date)
    VALUES
    (@student_id, @project_id, @ranking_precedence, @submitted_date)
END
GO
/****** Object:  StoredProcedure [dbo].[createStudentSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[createStudentSP]
    -- Add the parameters for the stored procedure here
    @admin_number CHAR(7),
    @full_name VARCHAR(50),
    @mobile_number CHAR(8),
    @email_address VARCHAR(50),
    @group_role CHAR(1),
    @year INT,
    @semester INT,
    @completed_module BIT,
    @group_id INT,
    @course_id INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    INSERT INTO Student (admin_number, full_name, mobile_number, email_address, group_role, year, semester, completed_module, group_id, course_id)
    VALUES
    (@admin_number, @full_name, @mobile_number, @email_address, @group_role, @year, @semester, @completed_module, @group_id, @course_id)
END
GO
/****** Object:  StoredProcedure [dbo].[deletePendingResponseGroupJoiningRequestsSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[deletePendingResponseGroupJoiningRequestsSP] 
    -- Add the parameters for the stored procedure here
    @studentId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    DELETE FROM Group_Joining_Request
    WHERE (student_id = @studentId AND (request_acceptance_date IS NULL AND notified_of_group_joining_request_outcome IS NULL))
END
GO
/****** Object:  StoredProcedure [dbo].[editLecturerSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[editLecturerSP] 
    -- Add the parameters for the stored procedure here
    @instaff_id CHAR(6),
       @infull_name VARCHAR(50),
       @incontact_number CHAR(8),
       @inemail_address VARCHAR(50)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    UPDATE Lecturer SET full_name=@infull_name,contact_number=@incontact_number,email_address=@inemail_address
    
    WHERE Lecturer.staff_id = @instaff_id
END
GO
/****** Object:  StoredProcedure [dbo].[editStudentSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[editStudentSP] 
    -- Add the parameters for the stored procedure here
    @student_id INT,
    @admin_number CHAR(8),
    @full_name VARCHAR(50),
    @mobile_number CHAR(8),
    @email_address VARCHAR(50),
    @group_role CHAR(1),
    @year CHAR(4),
    @semester INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE Student SET admin_number = @admin_number, full_name = @full_name, mobile_number = @mobile_number, email_address = @email_address, group_role = @group_role, year = @year, semester = @semester
    WHERE student_id = @student_id
END
GO
/****** Object:  StoredProcedure [dbo].[getGroupSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getGroupSP] 
    -- Add the parameters for the stored procedure here
    @course_id INT,
    @group_no INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY group_id ASC) AS "No",* FROM "Group" WHERE course_id = @course_id) AS O WHERE "No" = @group_no
END
GO
/****** Object:  StoredProcedure [dbo].[getIndustryLecturerProjectsByLecturerSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getIndustryLecturerProjectsByLecturerSP] 
    -- Add the parameters for the stored procedure here
    @lecturerId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT Project.*, Industry_Lecturer_Project.project_milestones, Industry_Lecturer_Project.company_name, Industry_Lecturer_Project.company_postal_address, Industry_Lecturer_Project.company_telephone_number, Industry_Lecturer_Project.company_fax_number, Industry_Lecturer_Project.company_email_address, Industry_Lecturer_Project.company_sponsorship_willingness, Industry_Lecturer_Project.company_liaison_officer_name, Industry_Lecturer_Project.deadline
    FROM Project, Industry_Lecturer_Project
    WHERE (lecturer_id = @lecturerId AND Project.project_id = Industry_Lecturer_Project.project_id)
END
GO
/****** Object:  StoredProcedure [dbo].[getIndustryLecturerProjectsSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getIndustryLecturerProjectsSP] 
    -- Add the parameters for the stored procedure here
    @courseId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT Project.*, Industry_Lecturer_Project.project_milestones, Industry_Lecturer_Project.company_name, Industry_Lecturer_Project.company_postal_address, Industry_Lecturer_Project.company_telephone_number, Industry_Lecturer_Project.company_fax_number, Industry_Lecturer_Project.company_email_address, Industry_Lecturer_Project.company_sponsorship_willingness, Industry_Lecturer_Project.company_liaison_officer_name, Industry_Lecturer_Project.deadline
    FROM Project, Industry_Lecturer_Project
    WHERE (course_id = @courseId AND Project.project_id = Industry_Lecturer_Project.project_id)
END
GO
/****** Object:  StoredProcedure [dbo].[getLecturersSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getLecturersSP] 
    -- Add the parameters for the stored procedure here
    @instaff_id CHAR(6) = NULL,
       @infull_name VARCHAR(50) = NULL,
       @incontact_number CHAR(8) = NULL,
       @inemail_address VARCHAR(50) = NULL,
       @inPageNo INT = 1,
       @inPageSize INT = 10,
       /*– Sorting Parameters */
       @inSortColumn NVARCHAR(20) = 'staff_id',
       @inSortOrder NVARCHAR(4)='ASC'
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    DECLARE 
         @lstaff_id CHAR(6),
         @lfull_name VARCHAR(50) = NULL,
         @lcontact_number CHAR(8) = NULL,
         @lemail_address VARCHAR(50),
         @lPageNbr INT,
         @lPageSize INT,
         @lSortCol NVARCHAR(20),
         @lFirstRec INT,
         @lLastRec INT,
         @lTotalRows INT
        /*Setting Local Variables*/
        SET @lstaff_id = LTRIM(RTRIM(@instaff_id))
        SET @lfull_name = LTRIM(RTRIM(@infull_name))
        SET @lcontact_number = LTRIM(RTRIM(@incontact_number))
        SET @lemail_address = LTRIM(RTRIM(@inemail_address))
        SET @lPageNbr = @inPageNo
        SET @lPageSize = @inPageSize
        SET @lSortCol = LTRIM(RTRIM(@inSortColumn))
         SET @lFirstRec = ( @lPageNbr - 1 ) * @lPageSize
         SET @lLastRec = ( @lPageNbr * @lPageSize + 1 )
         SET @lTotalRows = @lFirstRec - @lLastRec + 1
         ; WITH CTE_Results
         AS (
         SELECT ROW_NUMBER() OVER (ORDER BY
                 CASE WHEN (@lSortCol = 'staff_id' AND @inSortOrder='ASC')
                            THEN staff_id
                  END ASC,
                  CASE WHEN (@lSortCol = 'staff_id' AND @inSortOrder='DESC')
                           THEN staff_id
                  END DESC,
                  CASE WHEN (@lSortCol = 'full_name' AND @inSortOrder='ASC')
                            THEN full_name
                  END ASC,
                  CASE WHEN (@lSortCol = 'full_name' AND @inSortOrder='DESC')
                           THEN full_name
                  END DESC,
                  CASE WHEN (@lSortCol = 'contact_number' AND @inSortOrder='ASC')
                            THEN contact_number
                  END ASC,
                  CASE WHEN (@lSortCol = 'contact_number' AND @inSortOrder='DESC')
                           THEN contact_number
                  END DESC,
                 CASE WHEN (@lSortCol = 'email_address' AND @inSortOrder='ASC')
                          THEN email_address
                 END ASC,
                 CASE WHEN @lSortCol = 'email_address' AND @inSortOrder='DESC'
                         THEN email_address
                           
                END DESC
                
       ) AS ROWNUM,
       Count(*) over () AS TotalCount,
       full_name,
       contact_number,
       email_address,
       staff_id
       From Lecturer
   WHERE
         (@lstaff_id IS NULL OR staff_id LIKE '%' + @lstaff_id + '%')
         AND(@lfull_name IS NULL OR full_name LIKE '%' + @lfull_name + '%')
         AND(@lemail_address IS NULL OR email_address LIKE '%' + @lemail_address + '%')

)
SELECT
TotalCount,
       full_name,
       contact_number,
       email_address,
       staff_id
FROM CTE_Results AS CPC
WHERE
         ROWNUM > @lFirstRec
               AND ROWNUM < @lLastRec
 ORDER BY ROWNUM ASC
END
GO
/****** Object:  StoredProcedure [dbo].[getNumberOfGroupJoiningRequestsWithSameGroupIdAndStudentIdSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getNumberOfGroupJoiningRequestsWithSameGroupIdAndStudentIdSP] 
    -- Add the parameters for the stored procedure here
    @studentId INT,
    @groupId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT COUNT(*) AS 'number_of_group_joining_requests_with_same_group_id_and_student_id'
    FROM Group_Joining_Request
    WHERE (group_id = @groupId AND student_id = @studentId)
END
GO
/****** Object:  StoredProcedure [dbo].[getNumberOfLecturersWithSameEmailAddress]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getNumberOfLecturersWithSameEmailAddress] 
    -- Add the parameters for the stored procedure here
    @inemail_address VARCHAR(50),
       @outTotalCount INT OUTPUT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    DECLARE 
         @lemail_address VARCHAR(50),
         @lTotalCount INT
        /*Setting Local Variables*/
        SET @lemail_address = LTRIM(RTRIM(@inemail_address))
        SET @outTotalCount = (SELECT Count(*) AS TotalCount FROM Lecturer WHERE 
                   email_address = @lemail_address)
END
GO
/****** Object:  StoredProcedure [dbo].[getNumberOfLecturersWithSameEmailAddressSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getNumberOfLecturersWithSameEmailAddressSP] 
    -- Add the parameters for the stored procedure here
    @email_address VARCHAR(50)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT COUNT(*) AS 'number_of_lecturers_with_same_email_address'
    FROM Lecturer
    WHERE email_address = @email_address
END
GO
/****** Object:  StoredProcedure [dbo].[getNumberOfLecturersWithSameStaffIdSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getNumberOfLecturersWithSameStaffIdSP] 
    -- Add the parameters for the stored procedure here
    @staff_id CHAR(5)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT COUNT(*) AS 'number_of_lecturers_with_same_staff_id'
    FROM Lecturer
    WHERE staff_id = @staff_id
END
GO
/****** Object:  StoredProcedure [dbo].[getNumberOfStudentsWithPartialFullNameFilterSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getNumberOfStudentsWithPartialFullNameFilterSP] 
    -- Add the parameters for the stored procedure here
    @partial_full_name VARCHAR(50),
    @course_id INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT COUNT(*) AS 'number_of_students'
    FROM Student
    WHERE (full_name LIKE '%' + @partial_full_name + '%' AND course_id = @course_id)
END
GO
/****** Object:  StoredProcedure [dbo].[getNumberOfStudentsWithSameAdminNumberSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getNumberOfStudentsWithSameAdminNumberSP] 
    -- Add the parameters for the stored procedure here
    @admin_number CHAR(7)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT COUNT(*) AS 'number_of_students_with_same_admin_number'
    FROM Student
    WHERE admin_number = @admin_number
END
GO
/****** Object:  StoredProcedure [dbo].[getNumberOfStudentsWithSameEmailAddressSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getNumberOfStudentsWithSameEmailAddressSP] 
    -- Add the parameters for the stored procedure here
    @email_address VARCHAR(50)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT COUNT(*)
    FROM Student
    WHERE email_address = @email_address
END
GO
/****** Object:  StoredProcedure [dbo].[getPendingResponseGroupJoiningRequestsSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getPendingResponseGroupJoiningRequestsSP] 
    -- Add the parameters for the stored procedure here
    @groupId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT Group_Joining_Request.*, Student.admin_number, Student.full_name, Student.mobile_number, Student.email_address, Student.group_role,Student.year, Student.semester, Student.completed_module, Student.course_id, "Group".group_number, "Group".valid, "Group".lecturer_id, "Group".project_id
    FROM Group_Joining_Request, Student, "Group"
    WHERE (Group_Joining_Request.group_id = @groupId AND Group_Joining_Request.group_id = "Group".group_id AND Group_Joining_Request.student_id = Student.student_id AND Group_Joining_Request.request_acceptance_date IS NULL AND Group_Joining_Request.notified_of_group_joining_request_outcome IS NULL)
END
GO
/****** Object:  StoredProcedure [dbo].[getProspectiveGroupNumberSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getProspectiveGroupNumberSP] 
    -- Add the parameters for the stored procedure here
    @course_id INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF EXISTS (SELECT * FROM "Group" WHERE course_id = @course_id)
    BEGIN
    SELECT CONCAT(SUBSTRING(group_number, 1, 1), SUBSTRING(group_number, 2, 1) + 1) AS 'prospective_group_number' FROM (SELECT TOP 1 * FROM "Group" WHERE course_id = @course_id ORDER BY group_number DESC) AS Q
    END
    ELSE
    BEGIN
    SELECT 'G1' AS 'prospective_group_number' 
    END
END
GO
/****** Object:  StoredProcedure [dbo].[getRandomUrlEmbeddedAccountActivationToken]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getRandomUrlEmbeddedAccountActivationToken] 
    -- Add the parameters for the stored procedure here
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT CONVERT(VARCHAR(50), NEWID()) AS random_url_embedded_account_activation_token
END
GO
/****** Object:  StoredProcedure [dbo].[getRespondedGroupJoiningRequestsSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getRespondedGroupJoiningRequestsSP] 
    -- Add the parameters for the stored procedure here
    @studentId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT Group_Joining_Request.*, Student.admin_number, Student.full_name, Student.mobile_number, Student.email_address, Student.group_role, Student.year, Student.semester, Student.completed_module, Student.course_id, "Group".group_number, "Group".valid, "Group".lecturer_id, "Group".project_id
    FROM Group_Joining_Request, Student, "Group"
    WHERE (Group_Joining_Request.group_id = "Group".group_id AND Group_Joining_Request.student_id = Student.student_id AND Group_Joining_Request.request_acceptance_date IS NOT NULL AND Group_Joining_Request.notified_of_group_joining_request_outcome = 0 AND Group_Joining_Request.student_id = @studentId)
END
GO
/****** Object:  StoredProcedure [dbo].[getStudentProjectsSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[getStudentProjectsSP] 
    -- Add the parameters for the stored procedure here
    @courseId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT Project.*, Student_Project.project_background, Student_Project.strategy_and_approach, Student_Project.merits_comparison, Student_Project.commercialisation_strategy, Student_Project.project_milestones_and_workload_allocation, Student_Project.problems_and_countermeasures
    FROM Project, Student_Project
    WHERE (course_id = @courseId AND Student_Project.project_id = Project.project_id)

END
GO
/****** Object:  StoredProcedure [dbo].[getStudentsWithPartialFullNameFilterSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getStudentsWithPartialFullNameFilterSP] 
    -- Add the parameters for the stored procedure here
    @partial_full_name VARCHAR(50),
    @column_to_sort VARCHAR(100),
    @sorting_order VARCHAR(4),
    @start_index INT,
    @interval INT,
    @course_id INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT * FROM (
    SELECT ROW_NUMBER() OVER (
    ORDER BY
    CASE WHEN @sorting_order = 'ASC' THEN
    CASE @column_to_sort
    WHEN 'student_id' THEN student_id
    WHEN 'semester' THEN semester
    WHEN 'group_id' THEN group_id
    WHEN 'course_id' THEN course_id END
    END ASC,
    CASE WHEN @sorting_order = 'ASC' THEN
    CASE @column_to_sort
    WHEN 'admin_number' THEN admin_number
    WHEN 'mobile_number' THEN mobile_number
    WHEN 'group_role' THEN group_role
    WHEN 'year' THEN year END
    END ASC,
    CASE WHEN @sorting_order = 'ASC' THEN
    CASE @column_to_sort
    WHEN 'full_name' THEN full_name
    WHEN 'email_address' THEN email_address END

    END ASC,
    CASE WHEN @sorting_order = 'ASC' THEN
    CASE @column_to_sort
    WHEN 'completed_module' THEN completed_module END
    END ASC,
    CASE WHEN @sorting_order = 'DESC' THEN
    CASE @column_to_sort
    WHEN 'student_id' THEN student_id
    WHEN 'semester' THEN semester
    WHEN 'group_id' THEN group_id
    WHEN 'course_id' THEN course_id END
    END DESC,
    CASE WHEN @sorting_order = 'DESC' THEN
    CASE @column_to_sort
    WHEN 'admin_number' THEN admin_number
    WHEN 'mobile_number' THEN mobile_number
    WHEN 'group_role' THEN group_role
    WHEN 'year' THEN year END
    END DESC,
    CASE WHEN @sorting_order = 'DESC' THEN
    CASE @column_to_sort
    WHEN 'full_name' THEN full_name
    WHEN 'email_address' THEN email_address END
    END DESC,
    CASE WHEN @sorting_order = 'DESC' THEN
    CASE @column_to_sort
    WHEN 'completed_module' THEN completed_module END
    END DESC)
    AS Q, * FROM Student WHERE (full_name LIKE '%' + @partial_full_name + '%' AND course_id = @course_id)) AS O
    WHERE Q BETWEEN @start_index AND ((@interval + @start_index) - 1)
END
GO
/****** Object:  StoredProcedure [dbo].[rejectOtherGroupJoiningRequestsSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[rejectOtherGroupJoiningRequestsSP] 
    -- Add the parameters for the stored procedure here
    @studentId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    UPDATE Group_Joining_Request
    SET request_acceptance_date = '1970-01-01 00:00:00.000', notified_of_group_joining_request_outcome = 0
    WHERE (student_id = @studentId AND notified_of_group_joining_request_outcome IS NULL)
END
GO
/****** Object:  StoredProcedure [dbo].[setGroupLecturerIdSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[setGroupLecturerIdSP] 
    -- Add the parameters for the stored procedure here
    @group_id INT,
    @lecturer_id INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    UPDATE "Group"
    SET lecturer_id = @lecturer_id
    WHERE group_id = @group_id
END
GO
/****** Object:  StoredProcedure [dbo].[setGroupProjectIdSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[setGroupProjectIdSP] 
    -- Add the parameters for the stored procedure here
    @group_id INT,
    @project_id INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    UPDATE "Group"
    SET project_id = @project_id
    WHERE group_id = @group_id
END
GO
/****** Object:  StoredProcedure [dbo].[setStudentGroupRoleAndGroupIdSP]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Name
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[setStudentGroupRoleAndGroupIdSP] 
    -- Add the parameters for the stored procedure here
    @student_id INT,
    @group_id INT,
    @group_role CHAR(1)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    UPDATE Student
    SET group_id = @group_id, group_role = @group_role
    WHERE student_id = @student_id
END
GO
/****** Object:  StoredProcedure [dbo].[usp_addAnnouncement]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_addAnnouncement]
(
       @announcementDate DATETIME,
       @message VARCHAR(1000),
       @title VARCHAR(100),
       @lastEditDate DATETIME,
       @lecturerId INT,
       @courseId INT,
       @outAnnouncementId INT OUTPUT
)
AS
BEGIN
  INSERT INTO Announcement
  (announcement_date, message, title, last_edit_date, lecturer_id, course_id)
  VALUES
  (@announcementDate, @message, @title, @lastEditDate, @lecturerId, @courseId)
  SET   @outAnnouncementId = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[usp_addExternalProject]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_addExternalProject]
(
       @inLecturerId INT,
       @inCourseId INT,
       @inCompanyName VARCHAR(50),
       @inAddress VARCHAR(100),
       @inTel CHAR(8),
       @inFax CHAR(8),
       @inEmail VARCHAR(50),
       @inLiaisonOfficer VARCHAR(50),
       @inWillingToSponsor BIT,

       @inSchedule VARCHAR(500),
       @inDeadline DATETIME,

       /* Inherited from [dbo][Project]*/
       @inProjectAims VARCHAR(500),
       @inProjectObjectives VARCHAR(1000),
       @inTargetAudience VARCHAR(500),
       @inMainFunction VARCHAR(500),
       @inHardwareAndSoftwareRequirements VARCHAR(500),
       @inProjectTitle VARCHAR(50),

       @inSubmittedOn DATETIME NULL,
       @outDuplTitle INT OUTPUT 
)
AS
BEGIN
  SET @outDuplTitle = (Select Count(*) from (Select * from Project where project_title = @inProjectTitle) AS projects)
  if (@outDuplTitle = 0)
  BEGIN
  DECLARE @ProjectId INT
  INSERT Project(project_title, project_aims, project_objectives, target_audience,main_functions_and_deliverables, hardware_and_software_requirements, course_id, submitted_on)
  VALUES (@inProjectTitle, @inProjectAims, @inProjectObjectives,@inTargetAudience, @inMainFunction, @inHardwareAndSoftwareRequirements, @inCourseId, @inSubmittedOn)
  SET @ProjectId = (SELECT SCOPE_IDENTITY());
  INSERT Industry_Lecturer_Project(project_id,company_name, company_postal_address, company_telephone_number, company_fax_number ,company_email_address, company_liaison_officer_name, company_sponsorship_willingness, lecturer_id, project_milestones,deadline)
  VALUES (@ProjectId ,@inCompanyName, @inAddress, @inTel, @inFax, @inEmail, @inLiaisonOfficer, @inWillingToSponsor, @inLecturerId, @inSchedule,@inDeadline)
 END
END
GO
/****** Object:  StoredProcedure [dbo].[usp_addFile]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_addFile]
(
    @name VARCHAR(100),
    @extension VARCHAR(10), 
    @data VARBINARY(MAX),
    @announcement_id INT
)
AS
BEGIN
  INSERT INTO File_Resource(name, extension, data, announcement_id)
  VALUES
  (@name, @extension, @data, @announcement_id)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_addNewLecturer]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_addNewLecturer]
(
       @inStaffId CHAR(5),
       @inFullName VARCHAR(50),
       @inEmailAddress VARCHAR(50),
       @inContactNumber CHAR(8), 
       @inIsAdmin BIT,
       @outRandomString VARCHAR(50) OUTPUT
)
AS
BEGIN
  DECLARE @randomString VARCHAR(MAX);
  SET @randomString = CONVERT(varchar(50), NEWID());
  INSERT Lecturer(staff_id, full_name, contact_number, email_address, admin)
  VALUES (@inStaffId, @inFullName, @inContactNumber, @inEmailAddress, @inIsAdmin)
  SET @outRandomString = @randomString
END
GO
/****** Object:  StoredProcedure [dbo].[usp_addNewStudent]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Batch submitted through debugger: SQLQuery13.sql|7|0|C:\Users\Leslie\AppData\Local\Temp\~vsA499.sql
CREATE PROCEDURE [dbo].[usp_addNewStudent]
(
       @inFullName VARCHAR(50),
       @inAdmin CHAR(7),
       @inEmail VARCHAR(50),
       @inMobile CHAR(8),
       @course_id INT,
       @completed_module BIT,
       @outRandomString VARCHAR(50) OUTPUT
)
AS
BEGIN
    DECLARE @randomString VARCHAR(MAX);
    DECLARE @date DATETIME;
    DECLARE @year INT;
    DECLARE @semester INT;
    DECLARE @startDate1 DATETIME,
     @endDate1 DATETIME, 
     @startDate2 DATETIME,
     @endDate2 DATETIME;

    SET @date = GETDATE();
    SET @randomString = CONVERT(varchar(50), NEWID());
    SET @year = DATEPART(YEAR, @date);

    SET @startDate1 = DATEFROMPARTS(@year, 4, 1)
    SET @endDate1 = DATEFROMPARTS(@year, 10, 18)
    SET @startDate2 = DATEFROMPARTS(@year, 10, 19)
    SET @endDate2 = DATEFROMPARTS(@year, 3, 31)

    SET @semester = (CASE
        WHEN @date BETWEEN @startDate1 and @endDate1 THEN 1
         WHEN @date BETWEEN @startDate2 and @endDate2 THEN 2
         END)
    

    INSERT Student(full_name, admin_number, email_address, mobile_number, completed_module, year, semester, course_id)
    VALUES (@inFullName, @inAdmin, @inEmail, @inMobile, @completed_module, @randomString, @year, @semester, @course_id)
    
    SET @outRandomString = @randomString
END
GO
/****** Object:  StoredProcedure [dbo].[usp_addSelfInitiatedProject]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_addSelfInitiatedProject]
(
       /* We need the group id */
       @inGroupId INT,
       @inCourseId INT,
       @inIntroBackground VARCHAR(500),
       @inKeyInnovation VARCHAR(500),
       @inComparison VARCHAR(500),
       @inBusinessModel VARCHAR(500),
       @inProjectOverview VARCHAR(500),
       @inProjectPlan VARCHAR(500),
       @inProblemsAndCounterMeasure VARCHAR(500),

       /* Inherited from [dbo][Project]*/
       @inTargetAudience VARCHAR(500),
       @inMainFunction VARCHAR(500),
       @inHardwareAndSoftwareRequirements VARCHAR(500),
       @inProjectTitle VARCHAR(50),
       @inSavedDate DATETIME,
       @inSubmittedOn DATETIME,
       @outProjectId INT OUTPUT
)
AS
BEGIN
  DECLARE @ProjectId INT
  INSERT Project(project_title, target_audience, main_functions_and_deliverables, hardware_and_software_requirements, course_id, submitted_on, updated_at)
  VALUES (@inProjectTitle, @inTargetAudience, @inMainFunction, @inHardwareAndSoftwareRequirements, @inCourseId, @inSubmittedOn, @inSavedDate)
  SET @ProjectId = (SELECT SCOPE_IDENTITY());
  INSERT Student_Project(project_id, project_overview, project_background, strategy_and_approach, merits_comparison, commercialisation_strategy, project_milestones_and_workload_allocation, problems_and_countermeasures, group_id)
  VALUES (@ProjectId, @inProjectOverview ,@inIntroBackground, @inKeyInnovation, @inComparison, @inBusinessModel, @inProjectPlan, @inProblemsAndCounterMeasure, @inGroupId) 

 SET @outProjectId = @ProjectId
END
GO
/****** Object:  StoredProcedure [dbo].[usp_checkForDuplicateLecturer]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_checkForDuplicateLecturer]
(
       @inStaffId CHAR(5),
       @outTotalCount INT OUTPUT
)
AS
BEGIN
        DECLARE 
         @totalCount INT
        SET @outTotalCount = (SELECT Count(*) AS TotalCount FROM Lecturer WHERE 
                   staff_id = LTRIM(RTRIM(@inStaffId)))       

END
GO
/****** Object:  StoredProcedure [dbo].[usp_checkForDuplicateStudent]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_checkForDuplicateStudent]
(
       @inAdmin CHAR(7),
       @outTotalCount INT OUTPUT
)
AS
BEGIN
        DECLARE 
         @totalCount INT
        SET @outTotalCount = (SELECT Count(*) AS TotalCount FROM Student WHERE 
                   admin_number = LTRIM(RTRIM(@inAdmin)))       

END
GO
/****** Object:  StoredProcedure [dbo].[usp_deleteAnnouncement]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_deleteAnnouncement]
(
       @inAnnouncementId INT
)
AS
BEGIN
  DELETE FROM Announcement WHERE announcement_id = @inAnnouncementId
     
END
GO
/****** Object:  StoredProcedure [dbo].[usp_deleteProposal]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_deleteProposal]
(
       @inProposalId INT
)
AS
BEGIN
    Delete Project_Choice where project_id = @inProposalId
    Delete Industry_Lecturer_Project where project_id = @inProposalId
    Delete Project where project_id = @inProposalId
     
END
GO
/****** Object:  StoredProcedure [dbo].[usp_updateAnnouncement]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_updateAnnouncement]
(
       @inMessage VARCHAR(500),
       @inTitle VARCHAR(20) = NULL,
       @inLecturerId INT,
       @inAnnouncementId INT
)
AS
BEGIN
  UPDATE Announcement SET message = LTRIM(RTRIM(@inMessage)), title = LTRIM(RTRIM(@inTitle)), lecturer_id = @inLecturerId,
    last_edit_date = GETDATE()
    WHERE announcement_id = @inAnnouncementId
    
END
GO
/****** Object:  StoredProcedure [dbo].[usp_updateExternalProject]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_updateExternalProject]
(
       /* Personal Details */
       @inProjectId INT,
       @inCompanyName VARCHAR(50),
       @inAddress VARCHAR(100),
       @inTel CHAR(8),
       @inFax CHAR(8),
       @inEmailAddress VARCHAR(50),
       @inLiaisonOfficer VARCHAR(50),
       @inWillingToSponsor BIT,
       
       /* Project */
       @inTitle VARCHAR(50),
       @inAims VARCHAR(500),
       @inObjectives VARCHAR(500),
       @inAudience VARCHAR(500),
       @inMainFunction VARCHAR(500),
       @inRequirements VARCHAR(500),

       /* Industry Leader */
       @inSchedule VARCHAR(500),
       @inSubmittedDate DATETIME 
)
AS
BEGIN
  UPDATE Industry_Lecturer_Project SET 
   company_name =  LTRIM(RTRIM(@inCompanyName)),
   company_postal_address = LTRIM(RTRIM(@inAddress)),
   company_telephone_number = @inTel,
   company_fax_number = @inFax,
   company_email_address = LTRIM(RTRIM(@inEmailAddress)),
   company_liaison_officer_name = LTRIM(RTRIM(@inLiaisonOfficer)),
   company_sponsorship_willingness = @inWillingToSponsor,
   project_milestones = @inSchedule
   WHERE project_id = @inProjectId

   UPDATE Project SET
   project_title = LTRIM(RTRIM(@inTitle)),
   project_aims = LTRIM(RTRIM(@inAims)),
   project_objectives = LTRIM(RTRIM(@inObjectives)),
   target_audience = LTRIM(RTRIM(@inAudience)),
   main_functions_and_deliverables = LTRIM(RTRIM(@inMainFunction)),
   hardware_and_software_requirements = LTRIM(RTRIM(@inRequirements)),
   submitted_on = @inSubmittedDate
   WHERE project_id = @inProjectId
    
END
GO
/****** Object:  StoredProcedure [dbo].[usp_updateStudentProject]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_updateStudentProject]
(      
       /* Project */
       @inProjectId INT,
       @inTitle VARCHAR(50),
       @inTargetAudience VARCHAR(500),  
       @inMainFunction VARCHAR(500),      
       @inHardwareAndSoftwareRequirements VARCHAR(500),
       @inSavedDate DATETIME,
       @inSubmittedDate DATETIME, 

       /* Student project */
       @inOverview VARCHAR(500),
       @inBackground VARCHAR(500),
       @inKeyInnovationAndResearchGoal VARCHAR(500),
       @inComparisonOfTheMerits VARCHAR(500),
       @inBusinessModelAndMarketPotential VARCHAR(500),
       @inProjectPlan VARCHAR(500),
       @inProblemsAndCountermeasures VARCHAR(500)
)
AS
BEGIN
  UPDATE Student_Project SET 
   project_overview =  LTRIM(RTRIM(@inOverview)),
   project_background = LTRIM(RTRIM(@inBackground)),
   strategy_and_approach = LTRIM(RTRIM(@inKeyInnovationAndResearchGoal)),
   merits_comparison = LTRIM(RTRIM(@inComparisonOfTheMerits)),
   commercialisation_strategy = LTRIM(RTRIM(@inBusinessModelAndMarketPotential)),
   project_milestones_and_workload_allocation = LTRIM(RTRIM(@inProjectPlan)),
   problems_and_countermeasures =  LTRIM(RTRIM(@inProblemsAndCountermeasures))
   WHERE project_id = @inProjectId

   UPDATE Project SET
   project_title = LTRIM(RTRIM(@inTitle)),
   target_audience = LTRIM(RTRIM(@inTargetAudience)),
   main_functions_and_deliverables = LTRIM(RTRIM(@inMainFunction)),
   hardware_and_software_requirements = LTRIM(RTRIM(@inHardwareAndSoftwareRequirements)),
   submitted_on = @inSubmittedDate,
   updated_at = @inSavedDate
   WHERE project_id = @inProjectId
    
END
GO
/****** Object:  StoredProcedure [dbo].[usp_updateUserPasswordLink]    Script Date: 13/8/2015 7:09:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_updateUserPasswordLink]
(     
       @inUserType INT,
       @inUserId INT,
       @outRandomString VARCHAR(50) OUTPUT  
)
AS
BEGIN
DECLARE @randomString VARCHAR(50)
SET @randomString = CONVERT(varchar(50), NEWID());

IF (@inUserType = 3)
BEGIN
UPDATE Student SET url_embedded_account_activation_token = @randomString WHERE admin_number = @inUserId
END
ELSE IF (@inUserType = 1 OR @inUserType = 2)
BEGIN
UPDATE Lecturer SET url_embedded_account_activation_token = @randomString WHERE staff_id = @inUserId
END

SET @outRandomString = @randomString
END
GO

CREATE TABLE [dbo].[ApplicationRole]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(256) NOT NULL,
    [NormalizedName] NVARCHAR(256) NOT NULL
)

GO

CREATE INDEX [IX_ApplicationRole_NormalizedName] ON [dbo].[ApplicationRole] ([NormalizedName])

GO

CREATE TABLE [dbo].[ApplicationUser]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [UserName] NVARCHAR(256) NOT NULL,
    [NormalizedUserName] NVARCHAR(256) NOT NULL,
    [Email] NVARCHAR(256) NULL,
    [NormalizedEmail] NVARCHAR(256) NULL,
    [EmailConfirmed] BIT NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NULL,
    [PhoneNumber] NVARCHAR(50) NULL,
    [PhoneNumberConfirmed] BIT NOT NULL,
    [TwoFactorEnabled] BIT NOT NULL
)

GO

CREATE INDEX [IX_ApplicationUser_NormalizedUserName] ON [dbo].[ApplicationUser] ([NormalizedUserName])

GO

CREATE INDEX [IX_ApplicationUser_NormalizedEmail] ON [dbo].[ApplicationUser] ([NormalizedEmail])

GO

CREATE TABLE [dbo].[ApplicationUserRole]
(
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL
    PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_ApplicationUserRole_User] FOREIGN KEY ([UserId]) REFERENCES [ApplicationUser]([Id]),
    CONSTRAINT [FK_ApplicationUserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [ApplicationRole]([Id])
)