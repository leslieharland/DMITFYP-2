using Xceed.Words.NET;
using System;
using System.Linq;
using System.Drawing;

namespace WebApp.Formatters
{
    public class DocxFormatter
    {
        public DocX GetExternalProjectProposalTemplate()
        {

            string typeface = "Times New Roman";
            string defaultTypeface = "Calibri";
            int tableFontSize = 12;

            string company = "School of Digital Media and Infocomm Technology, Singapore Polytechnic";
            string title = "Proposal for 3rd Year External Project";

            string headerText = company + Environment.NewLine + title;


            DocX document = DocX.Create(@"\\Test.docx");
            var headLineFormat = new Formatting();
			headLineFormat.FontFamily = new Xceed.Words.NET.Font("Times New Roman");
            headLineFormat.Bold = true;
            headLineFormat.Size = 12;

            var personalInfoFormat = new Formatting();
			personalInfoFormat.FontFamily = new Xceed.Words.NET.Font("Times New Roman");
            personalInfoFormat.Size = 12;

            var bodyFormat = new Formatting();
			bodyFormat.FontFamily = new Xceed.Words.NET.Font("Arial");
            bodyFormat.Size = 12;

            Paragraph header = document.InsertParagraph(headerText, false, headLineFormat);
            header.Alignment = Alignment.center;

            document.InsertParagraph(Environment.NewLine);

            document.AddFooters();
			Footer footer_default = document.Footers.Odd;
            Paragraph footerText = footer_default.InsertParagraph();
            footerText.Append("Project Proposal Form (External) _new");


            document.InsertParagraph("Company Details", false, headLineFormat);

            // Add a Table to this document.
            Table t = document.AddTable(5, 3);
            t.Alignment = Alignment.left;
            t.Design = TableDesign.TableGrid;

            // Add content to this Table.
			t.Rows[0].Cells[0].Paragraphs.First().Append("Company Title:").Font(new FontFamily(typeface).ToString().ToString()).FontSize(tableFontSize);
            t.Rows[0].MergeCells(1, 2);
            CustomProperty company_name = new CustomProperty("CompanyName", "[CompanyName]");
            t.Rows[0].Cells[1].Paragraphs[0].InsertDocProperty(company_name, false, personalInfoFormat);

            t.Rows[1].Cells[0].Width = 250;
            t.Rows[1].Cells[1].Width = 600;
			t.Rows[1].Cells[0].Paragraphs.First().Append("Address:").Font(new FontFamily(typeface).ToString().ToString()).FontSize(tableFontSize);
            t.Rows[1].MergeCells(1, 2);
            CustomProperty address = new CustomProperty("Address", "[Address]");
            t.Rows[1].Cells[1].Paragraphs[0].InsertDocProperty(address, false, personalInfoFormat);

			t.Rows[2].Cells[0].Paragraphs.First().Append("Tel: ").Font(new FontFamily(typeface).ToString().ToString()).FontSize(tableFontSize);
			t.Rows[2].Cells[1].Paragraphs.First().Append("Fax: ").Font(new FontFamily(typeface).ToString().ToString()).FontSize(tableFontSize);
			t.Rows[2].Cells[2].Paragraphs.First().Append("Email: ").Font(new FontFamily(typeface).ToString().ToString()).FontSize(tableFontSize);
            CustomProperty tel = new CustomProperty("Tel", "[Tel]");
            CustomProperty fax = new CustomProperty("Fax", "[Fax]");
            CustomProperty email = new CustomProperty("Email", "[Email]");
            t.Rows[2].Cells[0].Paragraphs[0].InsertDocProperty(tel, false, personalInfoFormat);
            t.Rows[2].Cells[1].Paragraphs[0].InsertDocProperty(fax, false, personalInfoFormat);
            t.Rows[2].Cells[2].Paragraphs[0].InsertDocProperty(email, false, personalInfoFormat);

			t.Rows[3].Cells[0].Paragraphs.First().Append("Liaison Officer:").Font(new FontFamily(typeface).ToString().ToString()).FontSize(tableFontSize);
            t.Rows[3].MergeCells(1, 2);
            CustomProperty liaison_officer = new CustomProperty("LiaisonOfficer", "[LiaisonOfficer]");
            t.Rows[3].Cells[1].Paragraphs[0].InsertDocProperty(liaison_officer, false, personalInfoFormat);

			t.Rows[4].Cells[0].Paragraphs.First().Append("Are you willing to sponsor cash (minimum of $500) for our Graduation Prize or Graduation show?:").Font(new FontFamily(typeface).ToString().ToString()).FontSize(tableFontSize);
            t.Rows[4].MergeCells(0, 1);
            CustomProperty willing_to_sponsor = new CustomProperty("WillingToSponsor", "[WillingToSponsor]");
            t.Rows[4].Cells[1].Paragraphs[0].InsertDocProperty(willing_to_sponsor, false, personalInfoFormat);

            // Insert the Table into the document.
            document.InsertTable(t);

            document.InsertParagraph(Environment.NewLine);

            document.InsertParagraph("Project Outline", false, headLineFormat);

            Table project_outline = document.AddTable(7, 1);
            project_outline.Alignment = Alignment.left;
            project_outline.Design = TableDesign.TableGrid;

            project_outline.Rows[0].Height = 50;
            project_outline.Rows[0].Cells[0].Width = 850;
            project_outline.Rows[0].Cells[0].Paragraphs.First().Append("Title" + Environment.NewLine).Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize);
            CustomProperty project_title = new CustomProperty("ProjectTitle", "[ProjectTitle]");
            project_outline.Rows[0].Cells[0].Paragraphs[0].InsertDocProperty(project_title, false, bodyFormat);

            project_outline.Rows[1].Height = 100;
            Paragraph aims = project_outline.Rows[1].Cells[0].Paragraphs.First()
                .Append("Aims").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (Describe the aims of the project)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);

            CustomProperty project_aims = new CustomProperty("ProjectAims", "[ProjectAims]");
            aims.InsertDocProperty(project_aims, false, bodyFormat);

            project_outline.Rows[2].Height = 120;
            project_outline.Rows[2].Cells[0].Paragraphs.First()
			               .Append("Objectives").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (List the objectives of the project)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty project_objectives = new CustomProperty("ProjectObjectives", "[ProjectObjectives]");
            project_outline.Rows[2].Cells[0].Paragraphs[0].InsertDocProperty(project_objectives, false, bodyFormat);

            project_outline.Rows[3].Height = 120;
            project_outline.Rows[3].Cells[0].Paragraphs.First()
                .Append("Schedule").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (List the main activities of the 16-week project)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty project_schedule = new CustomProperty("ProjectSchedule", "[ProjectSchedule]");
            project_outline.Rows[3].Cells[0].Paragraphs[0].InsertDocProperty(project_schedule, false, bodyFormat);

            project_outline.Rows[4].Height = 80;
            project_outline.Rows[4].Cells[0].Paragraphs.First()
                .Append("Target Audience").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (Describe the target audience for the project)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty project_audience = new CustomProperty("ProjectAudience", "[ProjectAudience]");
            project_outline.Rows[4].Cells[0].Paragraphs[0].InsertDocProperty(project_audience, false, bodyFormat);

            project_outline.Rows[5].Height = 130;
            project_outline.Rows[5].Cells[0].Paragraphs.First()
                .Append("Main functions /Deliverables").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (List the main functions of the application and deliverables, usually a prototype)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty project_main_function = new CustomProperty("ProjectMainFunction", "[ProjectMainFunction]");
            project_outline.Rows[5].Cells[0].Paragraphs[0].InsertDocProperty(project_main_function, false, bodyFormat);

            project_outline.Rows[6].Height = 85;
            project_outline.Rows[6].Cells[0].Paragraphs.First()
                .Append("Hardware and software requirements").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (Describe/sketch the proposed hardware configuration and software required)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty project_requirements = new CustomProperty("ProjectRequirements", "[ProjectRequirements]");
            project_outline.Rows[6].Cells[0].Paragraphs[0].InsertDocProperty(project_requirements, false, bodyFormat);

            document.InsertTable(project_outline);

            return document;
        }

        public DocX GetInitiatedProjectProposal(DateTime date, string semester, int noOfColumns = 0)
        {
            string typeface = "Times New Roman";
            string defaultTypeface = "Calibri";
            int tableFontSize = 12;

            DocX document = DocX.Create(@"\\Test.docx");
            var headLineFormat = new Formatting();
			headLineFormat.FontFamily = new Xceed.Words.NET.Font("Times New Roman");
            headLineFormat.Bold = true;
            headLineFormat.Size = 12;

            var bodyFormat = new Formatting();
			bodyFormat.FontFamily = new Xceed.Words.NET.Font("Arial");
            bodyFormat.Size = 12;

            document.AddFooters();
			Footer footer_default = document.Footers.Odd;
            Paragraph footerText = footer_default.InsertParagraph();
            int yearPart = int.Parse(date.ToString("yy"));
            footerText.Append("Release AY " + yearPart + (yearPart + 1) + "S" + semester);

            document.InsertParagraph("Project Outline", false, headLineFormat);

            Table project_outline = null;
            // Columns not added (and not specified)
            if (noOfColumns == 0)
            {
                project_outline = document.AddTable(11, 1);
            }
            project_outline.Alignment = Alignment.left;
            project_outline.Design = TableDesign.TableGrid;

            project_outline.Rows[0].Height = 50;
            project_outline.Rows[0].Cells[0].Width = 850;
            project_outline.Rows[0].Cells[0].Paragraphs.First().Append("Title" + Environment.NewLine).Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize);
            CustomProperty project_title = new CustomProperty("ProjectTitle", "[ProjectTitle]");
            project_outline.Rows[0].Cells[0].Paragraphs[0].InsertDocProperty(project_title, false, bodyFormat);

            project_outline.Rows[1].Height = 100;
            Paragraph aims = project_outline.Rows[1].Cells[0].Paragraphs.First()
                .Append("Project Overview").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (Describe the aims and objectives of the project - abstract)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);

            CustomProperty project_overview = new CustomProperty("ProjectOverview", "[ProjectOverview]");
            aims.InsertDocProperty(project_overview, false, bodyFormat);

            project_outline.Rows[2].Height = 120;
            project_outline.Rows[2].Cells[0].Paragraphs.First()
                .Append("Intro/Background").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (Describe how the project idea come about or current problems)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty project_background = new CustomProperty("IntroBackground", "[IntroBackground]");
            project_outline.Rows[2].Cells[0].Paragraphs[0].InsertDocProperty(project_background, false, bodyFormat);

            project_outline.Rows[3].Height = 120;
            project_outline.Rows[3].Cells[0].Paragraphs.First()
                .Append("Key Innovation/Research Goals/Technical Approach").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (Describe what technology or research idea to be used)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty strategy_approach = new CustomProperty("Approach", "[Approach]");
            project_outline.Rows[3].Cells[0].Paragraphs[0].InsertDocProperty(strategy_approach, false, bodyFormat);

            project_outline.Rows[4].Height = 80;
            project_outline.Rows[4].Cells[0].Paragraphs.First()
                .Append("Comparison of the Merits").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (Describe the advantages/ improvements of the project vs. those of the current one)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty comparison_merits = new CustomProperty("ComparisonMerits", "[ComparisonMerits]");
            project_outline.Rows[4].Cells[0].Paragraphs[0].InsertDocProperty(comparison_merits, false, bodyFormat);

            project_outline.Rows[5].Height = 130;
            project_outline.Rows[5].Cells[0].Paragraphs.First()
                .Append("Target Audience").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (Describe the target audience for this project, e.g. mobile users, students, government agency, etc.)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty target_audience = new CustomProperty("TargetAudience", "[TargetAudience]");
            project_outline.Rows[5].Cells[0].Paragraphs[0].InsertDocProperty(target_audience, false, bodyFormat);

            project_outline.Rows[6].Height = 85;
            project_outline.Rows[6].Cells[0].Paragraphs.First()
                .Append("Business Model/ Market Potential").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (Describe how to market (or commercialise) your product if successful)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty business_and_market_potential = new CustomProperty("BusinessModel", "[BusinessModel]");
            project_outline.Rows[6].Cells[0].Paragraphs[0].InsertDocProperty(business_and_market_potential, false, bodyFormat);

            project_outline.Rows[7].Height = 85;
            project_outline.Rows[7].Cells[0].Paragraphs.First()
                .Append("Main function/ Deliverables").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (List the main functions of the application and deliverables (usually prototype), includes additional feature enhancement if time allows)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty main_function_and_deliverables = new CustomProperty("MainFunction", "[MainFunction]");
            project_outline.Rows[7].Cells[0].Paragraphs[0].InsertDocProperty(main_function_and_deliverables, false, bodyFormat);

            project_outline.Rows[8].Height = 85;
            project_outline.Rows[8].Cells[0].Paragraphs.First()
                .Append("Project Plan/ Schedule").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (List the main activities for the project by week(s), including manpower requirements - function distribution of group members)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty project_plan = new CustomProperty("ProjectPlan", "[ProjectPlan]");
            project_outline.Rows[8].Cells[0].Paragraphs[0].InsertDocProperty(project_plan, false, bodyFormat);

            project_outline.Rows[9].Height = 85;
            project_outline.Rows[9].Cells[0].Paragraphs.First()
                .Append("Hardware And Software Requirements").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (Describes/plan the proposed hardware configuration and software required)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty hardware_and_software_requirements = new CustomProperty("HardwareAndSoftwareRequirements", "[HardwareAndSoftwareRequirements]");
            project_outline.Rows[9].Cells[0].Paragraphs[0].InsertDocProperty(hardware_and_software_requirements, false, bodyFormat);

            project_outline.Rows[10].Height = 85;
            project_outline.Rows[10].Cells[0].Paragraphs.First()
                .Append("Problems and countermeasures").Bold().Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(" (Brief risk analysis - optional)").Font(new FontFamily(typeface).ToString()).FontSize(tableFontSize)
                .Append(Environment.NewLine);
            CustomProperty problems_and_countermeasures = new CustomProperty("ProblemsAndCountermeasures", "[ProblemsAndCountermeasures]");
            project_outline.Rows[10].Cells[0].Paragraphs[0].InsertDocProperty(problems_and_countermeasures, false, bodyFormat);

            document.InsertTable(project_outline);

            return document;
        }
    }
}