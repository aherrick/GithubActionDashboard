using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace GithubActionDashboard.Services
{
    public class GitHubService
    {
        private GitHubClient _client;
        private Dictionary<long, (string Owner, string Name)> _repoCache = new();

        public void Initialize(string token)
        {
            _client = new GitHubClient(new ProductHeaderValue("BlazorGithubDashboard"))
            {
                Credentials = new Credentials(token),
            };
            _repoCache.Clear();
        }

        public Task<IReadOnlyList<Repository>> GetRepositoriesAsync() => 
            _client.Repository.GetAllForCurrent();

        private async Task<(string Owner, string Name)> GetRepoInfoAsync(long repoId)
        {
            if (!_repoCache.ContainsKey(repoId))
            {
                var repo = await _client.Repository.Get(repoId);
                _repoCache[repoId] = (repo.Owner.Login, repo.Name);
            }
            return _repoCache[repoId];
        }

        public async Task<WorkflowsResponse> GetWorkflowsAsync(long repoId)
        {
            var (owner, name) = await GetRepoInfoAsync(repoId);
            return await _client.Actions.Workflows.List(owner, name);
        }

        public async Task TriggerWorkflowAsync(
            long repoId,
            long workflowId,
            string refName = "main"
        )
        {
            var (owner, name) = await GetRepoInfoAsync(repoId);
            var dispatch = new CreateWorkflowDispatch(refName);
            await _client.Actions.Workflows.CreateDispatch(owner, name, workflowId, dispatch);
        }

        public async Task<WorkflowRunsResponse> GetAllWorkflowRunsForRepoAsync(
            long repoId,
            int perPage = 100
        )
        {
            var (owner, name) = await GetRepoInfoAsync(repoId);
            var uri = new Uri($"repos/{owner}/{name}/actions/runs?per_page={perPage}", UriKind.Relative);

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