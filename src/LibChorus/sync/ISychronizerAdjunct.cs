using Palaso.Progress;
using System.Collections.Generic;
using Chorus.VcsDrivers.Mercurial;

namespace Chorus.sync
{
	/// <summary>
	/// Interface that allows Chorus clients to do something special at one or two points in a Send/Receive operation.
	///
	/// One point is right before the initial local commit.
	/// The other point is after an optional merge, but before its commit (cf. remarks and remarks on 'PrepareForPostMergeCommit' method).
	/// <remarks>
	/// NB: A merge is optional in a couple of ways:
	///		1. The client may not have asked for it to be done at all (e.g., WeSay's autosave, does not ask for the pull, merge, or push).
	///		2. Even if the client asks for a merge to be done, a merge may not actually be needed, so isn't done.
	/// </remarks>
	/// </summary>
	public interface ISychronizerAdjunct
	{
		/// <summary>
		/// Allow the client to do something right before the initial local commit.
		/// </summary>
		void PrepareForInitialCommit(IProgress progress);

		/// <summary>
		/// Allow the client to do something in one of two cases:
		///		1. User A had no new changes, but User B (from afar) did have them. No merge was done.
		///		2. There was a merge failure, so a rollback is being done.
		/// In both cases, the client may need to do something.
		/// </summary>
		///<param name="progress">A progress mechanism.</param>
		///<param name="isRollback">"True" if there was a merge failure, and the repo is being rolled back to an earlier state. Otherwise "False".</param>
		void SimpleUpdate(IProgress progress, bool isRollback);

		/// <summary>
		/// Allow the client to do something right after a merge, but before the merge is committed.
		/// </summary>
		/// <remarks>This method is not be called at all, if there was no merging.</remarks>
		void PrepareForPostMergeCommit(IProgress progress);

		/// <summary>
		/// Get the branch name the client wants to use. This might be (for example) a current version label
		/// of the client's data model. Used to create a version branch in the repository.
		/// </summary>
		string BranchName { get; }

		/// <summary>
		/// Gets a value telling if the adjunct processed anything.
		/// </summary>
		bool WasUpdated { get; }

		/// <summary>
		/// During a Send/Receive when Chorus has completed a pull and there is more than one branch on the repository
		/// it will pass the revision of the head of each branch to the client.
		/// The client can use this to display messages to the users when other branches are active other than their own.
		/// i.e. "Someone else has a new version you should update"
		/// or "Your colleague needs to update, you won't see their changes until they do."
		/// </summary>
		/// <param name="branches">A list (IEnumerable really) of all the open branches in this repo.</param>
		/// <param name="progress">So we can log a warning.</param>
		void CheckRepositoryBranches(IEnumerable<Revision> branches, IProgress progress);
	}
}