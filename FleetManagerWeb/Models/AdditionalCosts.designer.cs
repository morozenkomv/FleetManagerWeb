﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FleetManagerWeb.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="FleetManager")]
	public partial class AdditionalCostsDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertTripCostType(TripCostType instance);
    partial void UpdateTripCostType(TripCostType instance);
    partial void DeleteTripCostType(TripCostType instance);
    partial void InsertTrackerTripCost(TrackerTripCost instance);
    partial void UpdateTrackerTripCost(TrackerTripCost instance);
    partial void DeleteTrackerTripCost(TrackerTripCost instance);
    #endregion
		
		public AdditionalCostsDataContext() : 
				base(global::FleetManagerWeb.Properties.Settings.Default.FleetManagerConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public AdditionalCostsDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AdditionalCostsDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AdditionalCostsDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AdditionalCostsDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<TripCostType> TripCostTypes
		{
			get
			{
				return this.GetTable<TripCostType>();
			}
		}
		
		public System.Data.Linq.Table<TrackerTripCost> TrackerTripCosts
		{
			get
			{
				return this.GetTable<TrackerTripCost>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.InsertOrUpdateTripCostType")]
		public ISingleResult<InsertOrUpdateTripCostTypeResult> InsertOrUpdateTripCostType([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Id", DbType="BigInt")] System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Name", DbType="NVarChar(100)")] string name, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserId", DbType="BigInt")] System.Nullable<long> userId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="PageId", DbType="BigInt")] System.Nullable<long> pageId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, name, userId, pageId);
			return ((ISingleResult<InsertOrUpdateTripCostTypeResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SearchCostType")]
		public ISingleResult<SearchCostTypeResult> SearchCostType([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Rows", DbType="Int")] System.Nullable<int> rows, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Page", DbType="Int")] System.Nullable<int> page, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Search", DbType="NVarChar(500)")] string search, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Sort", DbType="NVarChar(50)")] string sort)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), rows, page, search, sort);
			return ((ISingleResult<SearchCostTypeResult>)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.TripCostType")]
	public partial class TripCostType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private EntitySet<TrackerTripCost> _TrackerTripCosts;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    #endregion
		
		public TripCostType()
		{
			this._TrackerTripCosts = new EntitySet<TrackerTripCost>(new Action<TrackerTripCost>(this.attach_TrackerTripCosts), new Action<TrackerTripCost>(this.detach_TrackerTripCosts));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NChar(100) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="TripCostType_TrackerTripCost", Storage="_TrackerTripCosts", ThisKey="Id", OtherKey="CostTypeId")]
		public EntitySet<TrackerTripCost> TrackerTripCosts
		{
			get
			{
				return this._TrackerTripCosts;
			}
			set
			{
				this._TrackerTripCosts.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_TrackerTripCosts(TrackerTripCost entity)
		{
			this.SendPropertyChanging();
			entity.TripCostType = this;
		}
		
		private void detach_TrackerTripCosts(TrackerTripCost entity)
		{
			this.SendPropertyChanging();
			entity.TripCostType = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.TrackerTripCost")]
	public partial class TrackerTripCost : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _CostTypeId;
		
		private double _Cost;
		
		private EntityRef<TripCostType> _TripCostType;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnCostTypeIdChanging(int value);
    partial void OnCostTypeIdChanged();
    partial void OnCostChanging(double value);
    partial void OnCostChanged();
    #endregion
		
		public TrackerTripCost()
		{
			this._TripCostType = default(EntityRef<TripCostType>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CostTypeId", DbType="Int NOT NULL")]
		public int CostTypeId
		{
			get
			{
				return this._CostTypeId;
			}
			set
			{
				if ((this._CostTypeId != value))
				{
					if (this._TripCostType.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnCostTypeIdChanging(value);
					this.SendPropertyChanging();
					this._CostTypeId = value;
					this.SendPropertyChanged("CostTypeId");
					this.OnCostTypeIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Cost", DbType="Float NOT NULL")]
		public double Cost
		{
			get
			{
				return this._Cost;
			}
			set
			{
				if ((this._Cost != value))
				{
					this.OnCostChanging(value);
					this.SendPropertyChanging();
					this._Cost = value;
					this.SendPropertyChanged("Cost");
					this.OnCostChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="TripCostType_TrackerTripCost", Storage="_TripCostType", ThisKey="CostTypeId", OtherKey="Id", IsForeignKey=true, DeleteOnNull=true, DeleteRule="CASCADE")]
		public TripCostType TripCostType
		{
			get
			{
				return this._TripCostType.Entity;
			}
			set
			{
				TripCostType previousValue = this._TripCostType.Entity;
				if (((previousValue != value) 
							|| (this._TripCostType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._TripCostType.Entity = null;
						previousValue.TrackerTripCosts.Remove(this);
					}
					this._TripCostType.Entity = value;
					if ((value != null))
					{
						value.TrackerTripCosts.Add(this);
						this._CostTypeId = value.Id;
					}
					else
					{
						this._CostTypeId = default(int);
					}
					this.SendPropertyChanged("TripCostType");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	public partial class InsertOrUpdateTripCostTypeResult
	{
		
		private long _InsertedId;
		
		public InsertOrUpdateTripCostTypeResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InsertedId", DbType="BigInt NOT NULL")]
		public long InsertedId
		{
			get
			{
				return this._InsertedId;
			}
			set
			{
				if ((this._InsertedId != value))
				{
					this._InsertedId = value;
				}
			}
		}
	}
	
	public partial class SearchCostTypeResult
	{
		
		private int _RowNum;
		
		private long _Id;
		
		private string _Name;
		
		private int _Total;
		
		public SearchCostTypeResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RowNum", DbType="Int NOT NULL")]
		public int RowNum
		{
			get
			{
				return this._RowNum;
			}
			set
			{
				if ((this._RowNum != value))
				{
					this._RowNum = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="BigInt NOT NULL")]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Total", DbType="Int NOT NULL")]
		public int Total
		{
			get
			{
				return this._Total;
			}
			set
			{
				if ((this._Total != value))
				{
					this._Total = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
