﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.Questions;

namespace ThiTracNghiem_BackEndAPI.Services.QuestionServices
{
    public interface IQuestionService
    {
        Task<ApiResult<QuestionRequest>> GetById(int questionId);
        Task<ApiResult<bool>> Delete(int questionId);
        Task<ApiResult<bool>> Update(QuestionRequest request, int questionId);
        Task<ApiResult<string>> Create(QuestionRequest request);
        Task<DatatableResult<List<QuestionViewModel>>> GetListQuestion(DatatableRequestBase requestBase);
    }
}
