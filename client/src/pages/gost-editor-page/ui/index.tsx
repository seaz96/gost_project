import classNames from "classnames";
import { useNavigate } from "react-router-dom";
import { axiosInstance } from "shared/configs/axiosConfig";
import type { newGostModel } from "widgets/gost-form";
import GostForm from "widgets/gost-form/ui";
import styles from "./GostEditorPage.module.scss";

const GostEditorPage = () => {
	const navigate = useNavigate();

	const addNewDocument = (gost: newGostModel.GostToSave, file: File) => {
		axiosInstance
			.post("/docs/add", gost)
			.then((response) => {
				handleUploadFile(file, response.data);
				return response.data;
			})
			.then((responce) => navigate("/gost-review/" + responce));
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

	return (
		<div className="container">
			<section className={classNames("contentContainer", styles.reviewSection)}>
				<GostForm handleUploadFile={handleUploadFile} handleSubmit={addNewDocument} />
			</section>
		</div>
	);
};

export default GostEditorPage;
