using Octokit;

namespace GithubActionDashboard.Services
{
    public class GitHubService
    {
        private GitHubClient _client;
        private IReadOnlyList<Repository> _repositories;

        public void Initialize(string token)
        {
            _client = new GitHubClient(new ProductHeaderValue("BlazorGithubDashboard"))
            {
                Credentials = new Credentials(token),
            };
            _repositories = null;
        }

        public async Task<IReadOnlyList<Repository>> GetRepositoriesAsync()
        {
            _repositories ??= await _client.Repository.GetAllForCurrent();
            return _repositories;
        }

        private async Task<(string Owner, string Name)> GetRepoInfoAsync(long repoId)
        {
            if (_repositories == null)
            {
                await GetRepositoriesAsync();
            }

            var repo = _repositories?.FirstOrDefault(r => r.Id == repoId);

            return (repo.Owner.Login, repo.Name);
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
            var uri = new Uri(
                $"repos/{owner}/{name}/actions/runs?per_page={perPage}",
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