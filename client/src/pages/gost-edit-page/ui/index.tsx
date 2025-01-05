import { useNavigate, useParams } from "react-router-dom";

import classNames from "classnames";
import type { gostModel } from "entities/gost";
import { axiosInstance } from "shared/configs/axiosConfig";
import GostForm from "../../../components/GostForm/GostForm.tsx";
import type { GostToSave } from "../../../components/GostForm/newGostModel.ts";
import useAxios from "../../../hooks/useAxios.ts";
import styles from "./GostEditPage.module.scss";

const GostEditPage = () => {
	const navigate = useNavigate();
	const id = useParams().id;
	const { response, loading, error } = useAxios<gostModel.Gost>(`/docs/${id}`);

	const editOldDocument = (gost: GostToSave, file: File) => {
		axiosInstance
			.put(`/docs/update/${id}`, gost)
			.then(() => handleUploadFile(file, id))
			.then(() => navigate(`/gost-review/${id}`));
	};

	const handleUploadFile = (file: File, docId: string | undefined) => {
		axiosInstance({
			method: "post",
			url: `/docs/${docId}/upload-file`,
			data: {
				File: file,
				Extension: file.name.split(".").pop(),
			},
			headers: { "Content-Type": "multipart/form-data" },
		});
	};

	if (loading) return <></>;

	if (response)
		return (
			<div className="container">
				<section className={classNames("contentContainer", styles.reviewSection)}>
					<GostForm
						handleUploadFile={handleUploadFile}
						handleSubmit={editOldDocument}
						gost={{
							...response.primary,
							references: response.references.map((reference) => reference.designation),
						}}
					/>
				</section>
			</div>
		);
	return <></>;
};

export default GostEditPage;