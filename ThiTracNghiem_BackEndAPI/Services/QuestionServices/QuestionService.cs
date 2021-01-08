using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_BackEndAPI.Models;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Questions;

namespace ThiTracNghiem_BackEndAPI.Services.QuestionServices
{
    public class QuestionService : IQuestionService
    {

        private readonly IConfiguration _configuration;
        private readonly tracnghiemContext _context;

        public QuestionService(IConfiguration configuration, tracnghiemContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<ApiResult<string>> Create(QuestionRequest request)
        {
            var questions = new Questions()
            {
                QuestionContent = request.QuestionContent,
                ModuleId = request.ModuleId,
                QuestionType = request.QuestionType,
                DateCreated = DateTime.Now,
            };
            var answer = new Answers()
            {
                A = request.A,
                B = request.B,
                C = request.C,
                D = request.D,
                CorrectAnswers = request.CorrectAnswers,
                AnswerExplain = request.AnswerExplain,
                Question = questions
            };
            _context.Questions.Add(questions);
            _context.Answers.Add(answer);
            var numRowChange = await _context.SaveChangesAsync();
            if(request.ExamId != 0)
            {
                var questionDetail = new Questiondetails()
                {
                    ExamId = request.ExamId,
                    QuestionId = questions.Id,
                };
                _context.Questiondetails.Add(questionDetail);
                await _context.SaveChangesAsync();
            }
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<string>("inserted");
            }
            else
            {
                return new ApiResultErrors<string>("Faild");
            }
        }

        public async Task<ApiResult<bool>> Delete(int questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            var answer = await _context.Answers.FirstOrDefaultAsync(a => a.QuestionId == questionId);
            if (question == null || answer == null ) return new ApiResultErrors<bool>($"Can not find question with id: {questionId}");
            _context.Answers.Remove(answer);
            _context.Questions.Remove(question);
            var numRowChange = await _context.SaveChangesAsync();
            if (numRowChange > 0)
            {
                return new ApiResultSuccess<bool>();
            }
            return new ApiResultErrors<bool>("Can not detete");
        }

        public async Task<ApiResult<QuestionRequest>> GetById(int questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            var answer = await _context.Answers.FirstOrDefaultAsync(a => a.QuestionId == questionId);
            if (question == null|| answer == null)
            {
                return new ApiResultErrors<QuestionRequest>("not found");
            }
            var questionViewModel = new QuestionRequest()
            {
                QuestionContent = question.QuestionContent,
                ModuleId = question.ModuleId,
                QuestionType = question.QuestionType,
                A = answer.A,
                B = answer.B,
                C = answer.C,
                D = answer.D,
                CorrectAnswers = answer.CorrectAnswers,
                AnswerExplain = answer.AnswerExplain,
            };
            return new ApiResultSuccess<QuestionRequest>(questionViewModel);
        }

        public async Task<int> GetCountTotalQuestion(int examId)
        {
            return await _context.Questiondetails.Where(x => x.ExamId == examId).CountAsync();
        }

        public async Task<DatatableResult<List<QuestionViewModel>>> GetListQuestion(DatatableRequestBase request)
        {
            var query = from r in _context.Questions
                        join m in _context.Modules on r.ModuleId equals m.Id
                        select new { r, m };

            if (!String.IsNullOrEmpty(request.searchValue))
            {
                query = query.Where(x => x.r.QuestionContent.Contains(request.searchValue));
            }

            int totalRow = await query.CountAsync();

            if (request.sortColumnDirection == "desc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(r => r.r.Id); break;
                    case "1": query = query.OrderByDescending(r => r.r.QuestionContent); break;
                    case "2": query = query.OrderByDescending(r => r.m.ModuleName); break;
                    case "3": query = query.OrderByDescending(r => r.r.QuestionType); break;
                    case "4": query = query.OrderByDescending(r => r.r.DateCreated); break;
                }

            }
            else if (request.sortColumnDirection == "asc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderBy(r => r.r.Id); break;
                    case "1": query = query.OrderBy(r => r.r.QuestionContent); break;
                    case "2": query = query.OrderBy(r => r.m.ModuleName); break;
                    case "3": query = query.OrderBy(r => r.r.QuestionType); break;
                    case "4": query = query.OrderBy(r => r.r.DateCreated); break;
                }

            }

            var data = await query.Skip(request.Skip).Take(request.PageSize)
                  .Select(x => new QuestionViewModel()
                  {
                      Id = x.r.Id,
                      QuestionContent = x.r.QuestionContent,
                      QuestionType = x.r.QuestionType,
                      DateCreated = x.r.DateCreated,
                      ModuleName = x.m.ModuleName,
                      Action = String.Format("<a href = 'javascript:void(0)' data-toggle = 'tooltip' id = 'edit' data-id = '{0}' data-original-title='Edit' class='btn mr-5 btn-xs btn-warning btn-edit'><i class='glyphicon glyphicon-edit'></i> Sửa</a><a href = 'javascript:void(0)' data-toggle='tooltip' id='delete' data-id='{1}' data-original-title='Delete' class='btn btn-xs btn-danger btn-delete'><i class='glyphicon glyphicon-trash'></i> Xóa</a>", x.r.Id, x.r.Id)
                  }).ToListAsync();

            var result = new DatatableResult<List<QuestionViewModel>>()
            {
                recordsTotal = totalRow,
                recordsFiltered = totalRow,
                Draw = request.Draw,
                Data = data
            };
            return result;
        }

        public async Task<DatatableResult<List<QuestionViewModel>>> GetListQuestionByExam(DatatableRequestBase request, int examId)
        {
            var query = from e in _context.Exams where e.Id == examId && e.Status ==1
                        join qd in _context.Questiondetails on e.Id equals qd.ExamId 
                        join q in _context.Questions on qd.QuestionId equals q.Id
                        join m in _context.Modules on q.ModuleId equals m.Id
                        select  new { q, m };

            if (!String.IsNullOrEmpty(request.searchValue))
            {
                query = query.Where(x => x.q.QuestionContent.Contains(request.searchValue));
            }

            int totalRow = await query.CountAsync();

            if (request.sortColumnDirection == "desc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(r => r.q.Id); break;
                    case "1": query = query.OrderByDescending(r => r.q.QuestionContent); break;
                    case "2": query = query.OrderByDescending(r => r.m.ModuleName); break;
                    case "3": query = query.OrderByDescending(r => r.q.DateCreated); break;
                }

            }
            else if (request.sortColumnDirection == "asc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderBy(r => r.q.Id); break;
                    case "1": query = query.OrderBy(r => r.q.QuestionContent); break;
                    case "2": query = query.OrderBy(r => r.m.ModuleName); break;
                    case "3": query = query.OrderBy(r => r.q.DateCreated); break;
                }

            }

            var data = await query.Skip(request.Skip).Take(request.PageSize)
                  .Select(x => new QuestionViewModel()
                  {
                      Id = x.q.Id,
                      QuestionContent = x.q.QuestionContent,
                      ModuleName = x.m.ModuleName,
                      DateCreated = x.q.DateCreated,
                      Action = String.Format("<a href = 'javascript:void(0)' data-toggle = 'tooltip' id = 'edit' data-id = '{0}' data-original-title='Edit' class='btn mr-5 btn-xs btn-warning btn-edit'><i class='glyphicon glyphicon-edit'></i> Sửa</a><a href = 'javascript:void(0)' data-toggle='tooltip' id='delete' data-id='{1}' data-original-title='Delete' class='btn btn-xs btn-danger btn-delete'><i class='glyphicon glyphicon-trash'></i> Xóa</a>", x.q.Id, x.q.Id)
                  }).ToListAsync();

            var result = new DatatableResult<List<QuestionViewModel>>()
            {
                recordsTotal = totalRow,
                recordsFiltered = totalRow,
                Draw = request.Draw,
                Data = data
            };
            return result;
        }

        public async Task<DatatableResult<List<QuestionViewModel>>> GetListQuestionByRoom(DatatableRequestBase request, int roomId)
        {
            var query = from r in _context.Rooms where r.Id == roomId
                        join qd in _context.Questiondetails on r.ExamId equals qd.ExamId
                        join q in _context.Questions on qd.QuestionId equals q.Id
                        select q;

            if (!String.IsNullOrEmpty(request.searchValue))
            {
                query = query.Where(x => x.QuestionContent.Contains(request.searchValue));
            }

            int totalRow = await query.CountAsync();

            if (request.sortColumnDirection == "desc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderByDescending(r => r.Id); break;
                    case "1": query = query.OrderByDescending(r => r.QuestionContent); break;
                    case "2": query = query.OrderByDescending(r => r.QuestionType); break;
                    case "3": query = query.OrderByDescending(r => r.DateCreated); break;
                }

            }
            else if (request.sortColumnDirection == "asc")
            {
                switch (request.sortColumn)
                {
                    case "0": query = query.OrderBy(r => r.Id); break;
                    case "1": query = query.OrderBy(r => r.QuestionContent); break;
                    case "2": query = query.OrderBy(r => r.QuestionType); break;
                    case "3": query = query.OrderBy(r => r.DateCreated); break;
                }

            }

            var data = await query.Skip(request.Skip).Take(request.PageSize)
                  .Select(x => new QuestionViewModel()
                  {
                      Id = x.Id,
                      QuestionContent = x.QuestionContent,
                      QuestionType = x.QuestionType,
                      DateCreated = x.DateCreated,
                      Action = String.Format("<a href = 'javascript:void(0)' data-toggle = 'tooltip' id = 'edit' data-id = '{0}' data-original-title='Edit' class='btn mr-5 btn-xs btn-warning btn-edit'><i class='glyphicon glyphicon-edit'></i> Sửa</a><a href = 'javascript:void(0)' data-toggle='tooltip' id='delete' data-id='{1}' data-original-title='Delete' class='btn btn-xs btn-danger btn-delete'><i class='glyphicon glyphicon-trash'></i> Xóa</a>", x.Id, x.Id)
                  }).ToListAsync();

            var result = new DatatableResult<List<QuestionViewModel>>()
            {
                recordsTotal = totalRow,
                recordsFiltered = totalRow,
                Draw = request.Draw,
                Data = data
            };
            return result;
        }

        public async Task<List<QuestionModuleViewModel>> GetQuestionByModule()
        {
            var query = from q in _context.Questions
                        join m in _context.Modules on q.ModuleId equals m.Id
                        group m by m.ModuleName into r
                        select new
                        {
                            count = r.Count(),
                            moduleName = r.Key
                        };
            var data = await query
                .Select(x => new QuestionModuleViewModel()
                {
                    ModuleName = x.moduleName,
                    Count = x.count
                }).ToListAsync();
            return data;
        }

        public async Task<List<QuestionModuleViewModel>> GetQuestionByModule(int ExamId)
        {
            var query = from e in _context.Exams where e.Id == ExamId && e.Status == 1
                        join qd in _context.Questiondetails on e.Id equals qd.ExamId 
                        join q in _context.Questions on qd.QuestionId equals q.Id
                         join m in _context.Modules on q.ModuleId equals m.Id
                        group m by m.ModuleName into r
                        select new
                        {
                            count = r.Count(),
                            moduleName = r.Key
                        };
            var data = await query
                .Select(x => new QuestionModuleViewModel()
                {
                    ModuleName = x.moduleName,
                    Count = x.count
                }).ToListAsync();
            return data;
        }

        public async Task<int> GetQuestionCount()
        {
            return await _context.Questions.CountAsync();
        }

        public async Task<ApiResult<bool>> Update(QuestionRequest request, int questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            var answer = await _context.Answers.FirstOrDefaultAsync(a => a.QuestionId == questionId);
            if (question == null || answer == null)
            {
                return new ApiResultErrors<bool>("not found");
            }
            question.QuestionContent = request.QuestionContent;
            question.ModuleId = request.ModuleId;
            question.QuestionType = request.QuestionType;

            answer.A = request.A;
            answer.B = request.B;
            answer.C = request.C;
            answer.D = request.D;
            answer.CorrectAnswers = request.CorrectAnswers;
            answer.AnswerExplain = request.AnswerExplain;

            await _context.SaveChangesAsync();
            return new ApiResultSuccess<bool>();
        }
    }
}
