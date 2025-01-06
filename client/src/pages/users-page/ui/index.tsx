import classNames from "classnames";
import UsersReview from "../../../components/UsersReview/UsersReview.tsx";
import { useFetchUsersQuery } from "../../../features/api/apiSlice";
import styles from "./UsersPage.module.scss";

const UsersPage = () => {
    const { data: users, isLoading } = useFetchUsersQuery();

    if (isLoading) return <></>;

    if (users)
        return (
            <div className="container">
                <section className={classNames("contentContainer", styles.gostSection)}>
                    <h2 className={styles.title}>Список пользователей</h2>
                    <UsersReview users={users} />
                </section>
            </div>
        );
    return <></>;
};

export default UsersPage;