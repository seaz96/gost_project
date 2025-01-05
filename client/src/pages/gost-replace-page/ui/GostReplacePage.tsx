import classNames from "classnames";
import { useNavigate, useParams } from "react-router-dom";

import type { gostModel } from "entities/gost";
import { axiosInstance } from "shared/configs/axiosConfig";
import { useAxios } from "shared/hooks";
import GostForm from "../../../widgets/gost-form/GostForm.tsx";
import type { GostToSave } from "../../../widgets/gost-form/newGostModel.ts";
import styles from "./GostReplacePage.module.scss";

function getGostStub() {
	return {
		designation: "",
		fullName: "",
		codeOks: "",
		activityField: "",
		acceptanceYear: "",
		commissionYear: "",
		author: "",
		acceptedFirstTimeOrReplaced: "",
		content: "",
		keyWords: "",
		applicationArea: "",
		adoptionLevel: 0,
		documentText: "",
		changes: "",
		amendments: "",
		status: 0,
		harmonization: 1,
		isPrimary: true,
		references: [],
	} as GostToSave;
}

const GostReplacePage = () => {
	const navigate = useNavigate();
	const gostToReplaceId = useParams().id;
	const { response, loading } = useAxios<gostModel.Gost>(`/docs/${gostToReplaceId}`);

	const addNewDocument = (gost: GostToSave, file: File) => {
		axiosInstance
			.post("/docs/add", gost)
			.then(() => {
				axiosInstance.put(`/docs/change-status`, {
					id: gostToReplaceId,
					status: 2,
				});
			})
			.then(() => {
				handleUploadFile(file, gostToReplaceId);
				return gostToReplaceId;
			})
			.then(() => navigate("/"));
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

	return (
		<div className="container">
			<section className={classNames("contentContainer", styles.reviewSection)}>
				<GostForm
					handleUploadFile={handleUploadFile}
					handleSubmit={addNewDocument}
					gost={{
						...getGostStub(),
						acceptedFirstTimeOrReplaced: `Принят взамен ${response?.primary.designation}`,
					}}
				/>
			</section>
		</div>
	);
};

export default GostReplacePage;
