using Octokit;

namespace GithubActionDashboard.Helpers;

public static class WorkflowStatusHelper
{
    public static string GetStatusBadgeClass(StringEnum<WorkflowRunStatus> status, StringEnum<WorkflowRunConclusion>? conclusion)
    {
        if (status.Value == WorkflowRunStatus.Completed)
        {
            return conclusion?.Value == WorkflowRunConclusion.Success ? "bg-success" : "bg-danger";
        }
        return "bg-warning text-dark";
    }
}
