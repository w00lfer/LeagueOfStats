using LeagueOfStats.Domain.Common.Rails.Results;
using MediatR;

namespace LeagueOfStats.Application.Tests.Common.TestCommons;

public sealed record DummyCommand()
    : IRequest<Result>;