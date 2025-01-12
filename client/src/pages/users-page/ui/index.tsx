import UsersReview from "../../../components/UsersReview/UsersReview.tsx";
import { useFetchUsersQuery } from "../../../features/api/apiSlice";

const UsersPage = () => {
	const { data: users, isLoading } = useFetchUsersQuery();

	if (isLoading) return <></>;

	if (users)
		return (
			<main className="container">
				<h1 className="verticalPadding">Список пользователей</h1>
				<section className="verticalPadding">
					<UsersReview users={users} />
				</section>
			</main>
		);
	return <></>;
};

export default UsersPage;
