using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace GithubActionDashboard.Services
{
    public class GitHubService
    {
        private GitHubClient _client;

        public void Initialize(string token)
        {
            _client = new GitHubClient(new ProductHeaderValue("BlazorGithubDashboard"))
            {
                Credentials = new Credentials(token),
            };
        }

        public async Task<IReadOnlyList<Repository>> GetRepositoriesAsync()
        {
            var allRepos = await _client.Repository.GetAllForCurrent();
            return allRepos;
        }

        public async Task<Repository> GetRepositoryAsync(long repoId)
        {
            return await _client.Repository.Get(repoId);
        }

        public async Task<WorkflowsResponse> GetWorkflowsAsync(long repoId)
        {
            var repo = await _client.Repository.Get(repoId);
            return await _client.Actions.Workflows.List(repo.Owner.Login, repo.Name);
        }

        public async Task TriggerWorkflowAsync(
            long repoId,
            string workflowFileNameOrId,
            string refName = "main"
        )
        {
            var dispatch = new CreateWorkflowDispatch(refName);
            var repo = await _client.Repository.Get(repoId);

            if (long.TryParse(workflowFileNameOrId, out long workflowId))
            {
                await _client.Actions.Workflows.CreateDispatch(
                    repo.Owner.Login,
                    repo.Name,
                    workflowId,
                    dispatch
                );
            }
            else
            {
                await _client.Actions.Workflows.CreateDispatch(
                    repo.Owner.Login,
                    repo.Name,
                    workflowFileNameOrId,
                    dispatch
                );
            }
        }

        public async Task<WorkflowRunsResponse> GetWorkflowRunsAsync(
            long repoId,
            string workflowFileNameOrId,
            int perPage = 100
        )
        {
            var repo = await _client.Repository.Get(repoId);

            // Use manual API call since Octokit overload seems tricky or missing in this context
            // Endpoint: GET /repos/{owner}/{repo}/actions/workflows/{workflow_id}/runs
            var uri = new Uri(
                $"repos/{repo.Owner.Login}/{repo.Name}/actions/workflows/{workflowFileNameOrId}/runs?per_page={perPage}",
                UriKind.Relative
            );

            try
            {
                var response = await _client.Connection.Get<WorkflowRunsResponse>(uri, null);
                return response.Body;
            }
            catch
            {
                return null;
            }
        }
    }
}