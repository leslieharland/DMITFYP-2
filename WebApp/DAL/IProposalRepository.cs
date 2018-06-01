using System.Collections.Generic;
using WebApp.Models;
using WebApp.ViewModels.Proposal;

namespace WebApp.DAL
{
    public interface IProposalRepository
    {
        IEnumerable<ProposalViewModel> GetForLecturer(Lecturer lecturer);
        IEnumerable<ProposalViewModel> Get(Lecturer lecturer, int saved = 0);
        dynamic GetProposalById(int Id);
        int AddProposal(ProposalViewModel proposal, int lecturerId, int courseId);
        void UpdateProposal(ProposalViewModel proposal);

        void DeleteProposals(List<int> Ids);
        void DeleteProposal(int Id);

        void ApproveProposal(int Id);
        void SubmitProposal(int Id);
    }
}