 
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.POS {

    public class I_POSSalesUploadSet {
        public List<IDGeneratorModel> IDGenerator { get; set; }
        public List<I_POSSaleUploadDoc> POSDocSet { get; set; }
        public I_BasicResult OutputAction { get; set; }

    }
    public class I_POSSaleUploadDoc {
        public POS_SaleHead Head { get; set; }
        public List<POS_SaleLine> Line { get; set; }
        public List<POS_SalePayment> Payment { get; set; }
        public List<TransactionLog> Log { get; set; }
        public I_BasicResult OutputAction { get; set; }
    }

    public class IDGeneratorModel : IDGenerator {
        public bool IsUpload { get; set; }

    }

}
