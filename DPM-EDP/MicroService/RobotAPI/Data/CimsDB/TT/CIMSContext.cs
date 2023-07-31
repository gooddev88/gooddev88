using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RobotAPI.Data;
using RobotAPI.Data.CimsDB.TT;

namespace RobotAPI.Data.DataStoreDB.TT {
    public partial class CIMSContext : DbContext {
        //public DataStoreContext()
        //{
        //} 
        //public DataStoreContext(DbContextOptions<DataStoreContext> options)
        //    : base(options)
        //{
        //} 

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { 
            optionsBuilder.UseNpgsql(Globals.CimsConn);
        }

        public CIMSContext() { }

        public CIMSContext(DbContextOptions<CIMSContext> options)
            : base(options) {
        }
      
        public virtual DbSet<a_province> a_province { get; set; }
        public virtual DbSet<accident_data> accident_data { get; set; }
        public virtual DbSet<api_connection> api_connection { get; set; }
        public virtual DbSet<api_datadict> api_datadict { get; set; }
        public virtual DbSet<api_master> api_master { get; set; }
        public virtual DbSet<api_param_res> api_param_res { get; set; }
        public virtual DbSet<api_cate> api_cate { get; set; }
        public virtual DbSet<api_tag> api_tag { get; set; }
        public virtual DbSet<board_authen> board_authen { get; set; }
        public virtual DbSet<board_in_user> board_in_user { get; set; }
        public virtual DbSet<board_master> board_master { get; set; }
 

        public virtual DbSet<custom_board_in_user> custom_board_in_user { get; set; }
        public virtual DbSet<custom_widget_master> custom_widget_master { get; set; }
        public virtual DbSet<custom_widget_in_user> custom_widget_in_user { get; set; }
        public virtual DbSet<custom_widget_param_in_user> custom_widget_param_in_user { get; set; }
        public virtual DbSet<mas_province> mas_province { get; set; }
 public virtual DbSet<trans_logs> trans_logs { get; set; }
        public virtual DbSet<vw_api_master> vw_api_master { get; set; }

      public virtual DbSet<vw_api_master_conn> vw_api_master_conn { get; set; }
        public virtual DbSet<widget_in_user> widget_in_user { get; set; }
        public virtual DbSet<widget_master> widget_master { get; set; }
        public virtual DbSet<vw_custom_board_in_user> vw_custom_board_in_user { get; set; }
        public virtual DbSet<vw_custom_widget_in_user> vw_custom_widget_in_user { get; set; }
        public virtual DbSet<vw_board_in_user> vw_board_in_user { get; set; }


        //public virtual DbSet<acd_accident_person> acd_accident_person { get; set; }
        //public virtual DbSet<dpm_disaster_report> dpm_disaster_report { get; set; }
        //public virtual DbSet<dpm_drought_risk> dpm_drought_risk { get; set; }
        //public virtual DbSet<dpm_flood_risk> dpm_flood_risk { get; set; }
        //public virtual DbSet<dqt_data_logs> dqt_data_logs { get; set; }
        //public virtual DbSet<dss_earthquake> dss_earthquake { get; set; }
        //public virtual DbSet<dss_rain> dss_rain { get; set; }
        //public virtual DbSet<dss_rid_dam> dss_rid_dam { get; set; }
        //public virtual DbSet<dss_typhoon> dss_typhoon { get; set; }
        //public virtual DbSet<dss_water> dss_water { get; set; }
        //public virtual DbSet<dss_weather_quality> dss_weather_quality { get; set; }
        //public virtual DbSet<mas_station> mas_station { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<a_province>(entity => {
                entity.Property(e => e.pcode).HasMaxLength(255);

                entity.Property(e => e.pname).HasMaxLength(255);

                entity.Property(e => e.type_p).HasMaxLength(255);

                entity.Property(e => e.type_soilder).HasMaxLength(255);
            });
            modelBuilder.Entity<accident_data>(entity => {
                entity.Property(e => e.id).ValueGeneratedNever();

                entity.Property(e => e.accident_cause).HasMaxLength(255);

                entity.Property(e => e.accident_date).HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.accident_no).HasMaxLength(255);

                entity.Property(e => e.accident_time).HasMaxLength(20);

                entity.Property(e => e.accident_timerange).HasMaxLength(255);

                entity.Property(e => e.alcohol_test_status).HasMaxLength(255);

                entity.Property(e => e.behaviors).HasMaxLength(255);

                entity.Property(e => e.branch).HasMaxLength(255);

                entity.Property(e => e.damage_value).HasMaxLength(255);

                entity.Property(e => e.death_place).HasMaxLength(255);

                entity.Property(e => e.district).HasMaxLength(255);

                entity.Property(e => e.domicile).HasMaxLength(255);

                entity.Property(e => e.guilty_of_law).HasMaxLength(255);

                entity.Property(e => e.has_driving_license).HasMaxLength(255);

                entity.Property(e => e.help_by).HasMaxLength(255);

                entity.Property(e => e.injured_status).HasMaxLength(255);

                entity.Property(e => e.is_bad_road).HasMaxLength(255);

                entity.Property(e => e.is_bad_vehicle).HasMaxLength(255);

                entity.Property(e => e.is_closecust).HasMaxLength(255);

                entity.Property(e => e.is_closecust2).HasMaxLength(255);

                entity.Property(e => e.is_crossroads).HasMaxLength(255);

                entity.Property(e => e.is_doze_off).HasMaxLength(255);

                entity.Property(e => e.is_doze_off2).HasMaxLength(255);

                entity.Property(e => e.is_drink).HasMaxLength(255);

                entity.Property(e => e.is_drink_and_drive).HasMaxLength(255);

                entity.Property(e => e.is_drive_backward).HasMaxLength(255);

                entity.Property(e => e.is_drive_backward2).HasMaxLength(255);

                entity.Property(e => e.is_illegal_overtaking).HasMaxLength(255);

                entity.Property(e => e.is_illegal_overtaking2).HasMaxLength(255);

                entity.Property(e => e.is_insure).HasMaxLength(255);

                entity.Property(e => e.is_invisible).HasMaxLength(255);

                entity.Property(e => e.is_island_road).HasMaxLength(255);

                entity.Property(e => e.is_motorcycle).HasMaxLength(255);

                entity.Property(e => e.is_ok_vehicle).HasMaxLength(255);

                entity.Property(e => e.is_other_cause).HasMaxLength(255);

                entity.Property(e => e.is_over_speed).HasMaxLength(255);

                entity.Property(e => e.is_over_speed2).HasMaxLength(255);

                entity.Property(e => e.is_over_weight).HasMaxLength(255);

                entity.Property(e => e.is_over_weight2).HasMaxLength(255);

                entity.Property(e => e.is_phone).HasMaxLength(255);

                entity.Property(e => e.is_phone2).HasMaxLength(255);

                entity.Property(e => e.is_roadblock).HasMaxLength(255);

                entity.Property(e => e.is_sit_back_pickup).HasMaxLength(255);

                entity.Property(e => e.is_substance_abuse).HasMaxLength(255);

                entity.Property(e => e.is_substance_abuse2).HasMaxLength(255);

                entity.Property(e => e.is_uturn).HasMaxLength(255);

                entity.Property(e => e.is_violating_traffic_lights).HasMaxLength(255);

                entity.Property(e => e.is_violating_traffic_lights2).HasMaxLength(255);

                entity.Property(e => e.is_violating_traffic_sign).HasMaxLength(255);

                entity.Property(e => e.is_violating_traffic_sign2).HasMaxLength(255);

                entity.Property(e => e.lat).HasMaxLength(255);

                entity.Property(e => e.lon).HasMaxLength(255);

                entity.Property(e => e.no_driving_license).HasMaxLength(255);

                entity.Property(e => e.no_helmet).HasMaxLength(255);

                entity.Property(e => e.no_seat_belts).HasMaxLength(255);

                entity.Property(e => e.number_of_lanes).HasMaxLength(255);

                entity.Property(e => e.paries_action).HasMaxLength(255);

                entity.Property(e => e.parties_name).HasMaxLength(255);

                entity.Property(e => e.parties_sex).HasMaxLength(255);

                entity.Property(e => e.parties_status).HasMaxLength(255);

                entity.Property(e => e.parties_type).HasMaxLength(255);

                entity.Property(e => e.paties_age).HasMaxLength(255);

                entity.Property(e => e.paties_agerange).HasMaxLength(255);

                entity.Property(e => e.paties_citizenid).HasMaxLength(255);

                entity.Property(e => e.paties_nationality).HasMaxLength(255);

                entity.Property(e => e.province).HasMaxLength(255);

                entity.Property(e => e.road).HasMaxLength(255);

                entity.Property(e => e.road_number).HasMaxLength(255);

                entity.Property(e => e.road_surface).HasMaxLength(255);

                entity.Property(e => e.road_type).HasMaxLength(255);

                entity.Property(e => e.safety_equipment).HasMaxLength(255);

                entity.Property(e => e.scene_of_road).HasMaxLength(255);

                entity.Property(e => e.special_event_notice).HasMaxLength(255);

                entity.Property(e => e.special_notice).HasMaxLength(255);

                entity.Property(e => e.subdistrict).HasMaxLength(255);

                entity.Property(e => e.vechicle_license).HasMaxLength(255);

                entity.Property(e => e.vehicle_subtype).HasMaxLength(255);

                entity.Property(e => e.vehicle_type).HasMaxLength(255);

                entity.Property(e => e.village).HasMaxLength(255);

                entity.Property(e => e.visibility).HasMaxLength(255);

                entity.Property(e => e.weather).HasMaxLength(255);
            });

            modelBuilder.Entity<api_connection>(entity => {
                entity.Property(e => e.id).UseIdentityAlwaysColumn();

                entity.Property(e => e.authen_type)
                    .HasMaxLength(255)
                    .HasComment("db /inurl / jwt / basic");
                 

                entity.Property(e => e.connection_code).HasMaxLength(255);

               

                entity.Property(e => e.api_password).HasMaxLength(255);

                entity.Property(e => e.api_token).HasMaxLength(255);

                entity.Property(e => e.api_username).HasMaxLength(255);
            });
 
            modelBuilder.Entity<custom_board_in_user>(entity => {
                entity.Property(e => e.board_desc).HasMaxLength(255);

                entity.Property(e => e.board_id).HasMaxLength(255);

                entity.Property(e => e.board_name).HasMaxLength(255);

                entity.Property(e => e.board_type).HasMaxLength(255);

                entity.Property(e => e.created_by).HasMaxLength(255);

                entity.Property(e => e.created_date).HasColumnType("timestamp without time zone");

                entity.Property(e => e.modified_by).HasMaxLength(255);

                entity.Property(e => e.modified_date).HasColumnType("timestamp without time zone");

                entity.Property(e => e.username).HasMaxLength(255);
            });

            modelBuilder.Entity<custom_widget_master>(entity => {
                entity.Property(e => e.widget_desc).HasMaxLength(255);

                entity.Property(e => e.widget_id).HasMaxLength(255);
            });

            modelBuilder.Entity<custom_widget_in_user>(entity => {
                entity.Property(e => e.board_id).HasMaxLength(255);


                entity.Property(e => e.username).HasMaxLength(255);

                entity.Property(e => e.widget_id).HasMaxLength(255);
            });

            modelBuilder.Entity<custom_widget_param_in_user>(entity => {
                entity.Property(e => e.data).HasMaxLength(1000);

                entity.Property(e => e.data_type).HasMaxLength(255);

                entity.Property(e => e.param_id).HasMaxLength(255);

                entity.Property(e => e.username).HasMaxLength(255);
            });

            modelBuilder.Entity<vw_custom_board_in_user>(entity => {
                entity.HasNoKey();

                entity.ToView("vw_custom_board_in_user");

                entity.Property(e => e.board_desc).HasMaxLength(255);

                entity.Property(e => e.board_id).HasMaxLength(255);

                entity.Property(e => e.board_name).HasMaxLength(255);

                entity.Property(e => e.board_type).HasMaxLength(255);

                entity.Property(e => e.username).HasMaxLength(255);
            });
            modelBuilder.Entity<board_authen>(entity => {
                entity.Property(e => e.authen_id).HasMaxLength(100);

                entity.Property(e => e.authen_name).HasMaxLength(255);

                entity.Property(e => e.base_url).HasMaxLength(255);

                entity.Property(e => e.board_pass).HasMaxLength(255);

                entity.Property(e => e.board_username).HasMaxLength(255);
            });

            modelBuilder.Entity<board_master>(entity => {
                entity.Property(e => e.authen_id).HasMaxLength(100);

                entity.Property(e => e.board_id)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.board_type).HasMaxLength(50);

                entity.Property(e => e.board_url).HasMaxLength(500);

                entity.Property(e => e.description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.img_path).HasMaxLength(255);

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.page).HasMaxLength(255);
            });

            modelBuilder.Entity<vw_board_in_user>(entity => {
                entity.HasNoKey();

                entity.ToView("vw_board_in_user");

                entity.Property(e => e.authen_name).HasMaxLength(255);

                entity.Property(e => e.base_url).HasMaxLength(255);

                entity.Property(e => e.board_description).HasMaxLength(1000);

                entity.Property(e => e.board_id).HasMaxLength(50);

                entity.Property(e => e.board_name).HasMaxLength(200);

                entity.Property(e => e.board_pass).HasMaxLength(255);

                entity.Property(e => e.board_type).HasMaxLength(50);

                entity.Property(e => e.board_url).HasMaxLength(500);

                entity.Property(e => e.board_username).HasMaxLength(255);

                entity.Property(e => e.layout_json_h).HasColumnType("character varying");

                entity.Property(e => e.layout_json_v).HasColumnType("character varying");

                entity.Property(e => e.username).HasColumnType("character varying");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
