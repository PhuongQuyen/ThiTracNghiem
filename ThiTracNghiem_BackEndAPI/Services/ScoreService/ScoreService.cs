using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_BackEndAPI.Models;
using ThiTracNghiem_ViewModel.Score;

namespace ThiTracNghiem_BackEndAPI.Services.ScoreService
{
    public class ScoreService : IScoreService
    {
        private readonly IConfiguration _configuration;
        private readonly tracnghiemContext _context;
        public ScoreService(IConfiguration configuration, tracnghiemContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<int> GetScoreCount()
        {
            return await _context.Scores.CountAsync();
        }

        public async Task<List<TopScoreViewModel>> GetTopFiveExamAsync()
        {
            var query = from s in _context.Scores
                        join e in _context.Exams on s.ExamId equals e.Id
                        join u in _context.Users on s.UserId equals u.Id
                        orderby s.Date descending
                        select new { s, e, u };
            var data = await query.Take(5)
                 .Select(x => new TopScoreViewModel()
                 {
                   ExamId = x.e.Id,
                   UserName = String.Format("{0} - {1} {2}",x.u.Id,x.u.FirstName,x.u.LastName),
                   ExamTitle = x.e.ExamTitle,
                   TotalQuestions = x.e.TotalQuestions,
                   TotalQuestionsCorrect = x.s.Score,
                   TestDate = x.s.Date
                 }).ToListAsync();
            return data;
        }
    }
}
