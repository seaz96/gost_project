import {useState} from "react";
import FilterTabs from "../../../components/FilterTabs/FilterTabs.tsx";
import {ChangesStatisticPage} from "./changes";
import {ReviewsStatisticPage} from "./reviews";

const StatisticsPage = () => {
	const [activeTabs, setActiveTabs] = useState<string>("reviews");
	const tabs = [
		{
			value: "reviews",
			title: "Обращения",
		},
		{
			value: "changes",
			title: "Стандарты",
		},
	];

	return (
		<div className="container">
			<h1 className="verticalPadding">Статистика</h1>
			<section className="verticalPadding">
				<FilterTabs
					tabs={tabs}
					activeTabs={[activeTabs]}
					setActiveTabs={(activeTabs) => setActiveTabs(activeTabs[0])}
				/>
			</section>
			<section>{activeTabs === "reviews" ? <ReviewsStatisticPage /> : <ChangesStatisticPage />}</section>
		</div>
	);
};

export default StatisticsPage;