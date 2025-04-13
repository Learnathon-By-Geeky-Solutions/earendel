using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Get 
{

    public record GetRandomQuizQuestionQuery(
        Guid AttemptId 
    ) : IRequest<IResult>; 
}