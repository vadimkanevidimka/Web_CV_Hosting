using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Entities;

namespace ProfileService.Infastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<IBaseEntity> TrackChanges(this IQueryable<IBaseEntity> tests, bool trackChanges)
    {
        if (trackChanges)
            return tests;

        return tests.AsNoTracking();
    }
    
    public static IQueryable<TDestination> ProjectTo<TDestination, TSource>(this IQueryable<TSource> queryable, IMapper mapper)
    {
        return mapper.ProjectTo<TDestination>(queryable);
    }

}