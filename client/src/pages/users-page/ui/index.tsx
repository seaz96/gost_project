import classNames from "classnames";
import type { userModel } from "entities/user";
import { useAxios } from "shared/hooks";
import UsersReview from "../../../widgets/users-review/UsersReview.tsx";
import styles from "./UsersPage.module.scss";

const UsersPage = () => {
	const { response, loading, error } = useAxios<userModel.User[]>("/accounts/list");

	if (loading) return <></>;

	if (response)
		return (
			<div className="container">
				<section className={classNames("contentContainer", styles.gostSection)}>
					<h2 className={styles.title}>Список пользователей</h2>
					<UsersReview users={response} />
				</section>
			</div>
		);
	return <></>;
};

export default UsersPage;