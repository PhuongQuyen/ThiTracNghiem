using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_ViewModel.Score;

namespace ThiTracNghiem_BackEndAPI.Services.ScoreService
{
    public interface IScoreService
    {
        public Task<List<TopScoreViewModel>> GetTopFiveExamAsync();
        Task<int> GetScoreCount();
    }
}
