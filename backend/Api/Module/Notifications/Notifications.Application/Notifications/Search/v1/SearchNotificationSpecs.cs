﻿using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Notifications.Application.Notifications.Get.v1;
using TalentMesh.Module.Notifications.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Notifications.Application.Notifications.Search.v1;

[ExcludeFromCodeCoverage]
public class SearchNotificationSpecs : EntitiesByPaginationFilterSpec<Notification, NotificationResponse>
{
    public SearchNotificationSpecs(SearchNotificationsCommand command)
        : base(command)
    {
        Query
            .Where(i => !command.UserId.HasValue || i.UserId == command.UserId.Value);

        // Single order chain with conditional default
        if (!command.HasOrderBy())
        {
            Query.OrderByDescending(n => n.Created)
                 .ThenBy(n => n.Entity);
        }
    }
}
