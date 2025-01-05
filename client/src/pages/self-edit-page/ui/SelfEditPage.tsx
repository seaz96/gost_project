import type { userModel } from "entities/user";
import { useNavigate } from "react-router-dom";

import { axiosInstance } from "shared/configs/axiosConfig";
import UserEditForm from "../../../components/UserEditForm/UserEditForm.tsx";
import type { UserEditType } from "../../../components/UserEditForm/userEditModel.ts";
import useAxios from "../../../hooks/useAxios.ts";
import styles from "./SelfEditPage.module.scss";

const SelfEditPage = () => {
	const navigate = useNavigate();
	const { response, loading, error } = useAxios<userModel.User>("/accounts/self-info");

	const handleSelfEdit = (userData: UserEditType) => {
		axiosInstance.post("/accounts/self-edit", userData).then(() => navigate("/users-page"));
	};

	if (response)
		return (
			<div>
				<section className={styles.userEditSection}>
					<UserEditForm handleSubmit={handleSelfEdit} userData={response} id={response.id} />
				</section>
			</div>
		);
	return <></>;
};

export default SelfEditPage;