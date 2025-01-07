import { useNavigate, useParams } from "react-router-dom";
import UserEditForm from "../../../components/UserEditForm/UserEditForm.tsx";
import type { UserEditType } from "../../../components/UserEditForm/userEditModel.ts";
import { useEditUserMutation, useFetchUserInfoQuery, useToggleAdminMutation } from "../../../features/api/apiSlice";
import styles from "./UserEditPage.module.scss";

const UserEditPage = () => {
	const navigate = useNavigate();
	const id = Number.parseInt(useParams().id || "");
	const { data: user, isLoading } = useFetchUserInfoQuery(id);
	const [editUser] = useEditUserMutation();
	const [toggleAdmin] = useToggleAdminMutation();

	const handleUserEdit = async (userData: UserEditType) => {
		await editUser({ ...userData, id });
		const isAdmin = user?.role === "Admin" || user?.role === "Heisenberg";
		if (userData.is_admin !== isAdmin && user) {
			await toggleAdmin({
				userId: user?.id,
				isAdmin: userData.is_admin && !isAdmin,
			});
		}
		navigate("/users-page");
	};

	if (isLoading) return <></>;

	if (user)
		return (
			<div>
				<section className={styles.userEditSection}>
					<UserEditForm handleSubmit={handleUserEdit} userData={user} id={user.id} />
				</section>
			</div>
		);
	return <></>;
};

export default UserEditPage;
