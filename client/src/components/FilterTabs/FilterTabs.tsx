import styles from "./FilterTabs.module.scss";

interface FilterTabsProps {
	tabs: {
		title: string;
		value: string;
	}[];
	activeTabs: string[];
	setActiveTabs?: (activeTabs: string[]) => void;
}

const FilterTabs = ({ tabs, activeTabs = [], setActiveTabs }: FilterTabsProps) => {
	return (
		<div className={styles.filterTabsContainer}>
			{tabs.map((tab) => (
				<div
					key={tab.value}
					className={`${styles.tab} ${activeTabs.includes(tab.value) ? styles.active : ""}`}
					onClick={() => setActiveTabs?.([tab.value])}
					onKeyDown={(e) => {
						if (e.key === "Enter" || e.key === " ") setActiveTabs?.([tab.value]);
					}}
				>
					{tab.title}
				</div>
			))}
		</div>
	);
};

export default FilterTabs;
